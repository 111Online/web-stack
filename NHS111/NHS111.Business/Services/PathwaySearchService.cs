using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Business.PathwaySearch;

namespace NHS111.Business.Services
{
    public class PathwaySearchService : IPathwaySearchService
    {
        private readonly IElasticClient _elastic;

        public PathwaySearchService(IConfiguration _configuration)
        {
            _elastic =
                new ElasticClient(
                    _configuration.GetElasticClientSettings().DisableDirectStreaming().OnRequestCompleted(details =>
                    {
                        Debug.WriteLine("### ES REQEUST ###");
                        if (details.RequestBodyInBytes != null)
                            Debug.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                        Debug.WriteLine("### ES RESPONSE ###");
                        if (details.ResponseBodyInBytes != null)
                            Debug.WriteLine(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
                    }));
        }

        public PathwaySearchService(IElasticClient elastic)
        {
            _elastic = elastic;
        }

        public async Task<List<PathwaySearchResult>> FindResults(string query, bool highlight, bool score)
        {
            var bodytagsResponse = await _elastic.SearchAsync<BodytagResult>(s => BuildBodytagsQuery(s.Index("bodytags"), Uri.UnescapeDataString(query)));

            var res = await _elastic.SearchAsync<PathwaySearchResult>(s =>
                    BuildPathwaysTextQuery(s.Index("pathways"), Uri.UnescapeDataString(query), bodytagsResponse.Hits)
                );
            
            var highlightedResults = BuildHighlights(res.Hits, highlight);

            return BuildScoredResults(highlightedResults, res.Hits, score).ToList();
        }

        public async Task<List<PathwaySearchResult>> FindResults(string query, string gender, string ageGroup, bool highlight, bool score)
        {
            
            var bodytagsResponse = await _elastic.SearchAsync<BodytagResult>(s => BuildBodytagsQuery(s.Index("bodytags"), Uri.UnescapeDataString(query)));

            var res = await _elastic.SearchAsync<PathwaySearchResult>(s =>
                AddAgeGenderFilters(BuildPathwaysTextQuery(s.Index("pathways"), Uri.UnescapeDataString(query), bodytagsResponse.Hits), gender, ageGroup));

            var highlightedResults = BuildHighlights(res.Hits, highlight);
            
            return BuildScoredResults(highlightedResults, res.Hits, score).ToList();
        }


        private IEnumerable<PathwaySearchResult> BuildScoredResults(IEnumerable<PathwaySearchResult> results, IReadOnlyCollection<IHit<PathwaySearchResult>> hits, bool includeScores)
        {
            if (includeScores)
            {
                foreach (var result in results)
                    result.Score = hits.First(h => h.Id == result.PathwayNo).Score;
            }

            return results;
        }

        private IEnumerable<PathwaySearchResult> BuildHighlights(IReadOnlyCollection<IHit<PathwaySearchResult>> hits, bool inlcudeHighlights)
        {
            if (inlcudeHighlights)
            {
                // hack - sorry James
                //hopefully you'll have a better way :-)
                foreach (var hit in hits)
                {
                    if (!hit.Highlights.Any()) continue;

                    foreach (var highlight in hit.Highlights)
                    {
                        switch (highlight.Key)
                        {
                            case "KP_Use":
                                hit.Source.Description = highlight.Value.Highlights.FirstOrDefault();
                                break;

                            case "DigitalDescriptions":
                                hit.Source.DisplayTitle = hit.Source.Title.Select(t => TitleOrHighLight(t, highlight.Value.Highlights)).ToList();
                                break;
                        }

                    }
                }
            }

            return hits.Select(h => h.Source);
        }

        private string TitleOrHighLight(string title, IReadOnlyCollection<string> highlights) {
            var highlightedTitle = highlights.FirstOrDefault(t=> title == PathwaySearchResult.StripHighlightMarkup(t));
            return highlightedTitle != null ? highlightedTitle : title;
        }

        private SearchDescriptor<BodytagResult> BuildBodytagsQuery(
            SearchDescriptor<BodytagResult> searchDescriptor, string query)
        {
            var bodytagQuery = searchDescriptor.Query(q => q
                .MultiMatch(m => 
                    m.Query(query)
                    .Fields(f => f.Field("tag"))
                ));

            return bodytagQuery;
        }


        private SearchDescriptor<PathwaySearchResult> BuildPathwaysTextQuery(
            SearchDescriptor<PathwaySearchResult> searchDescriptor, string query, IReadOnlyCollection<IHit<BodytagResult>> bodytags)
        {
            // boost exact matches on parent and child documents
            // then fuzzy match
            // slop value scores higher where more than one words from the query are found in a document (I THINK!)
            var shouldQuery = searchDescriptor.Query(q => q
                    .Boosting(qb => qb
                        .Positive(pos => pos
                            .Bool(b => b
                                .Must
                                (
                                    AddBodytags(query, bodytags)
                                )
                                .Should
                                (
                                    AddTitleAndDerscriptionMatchingQuery(query, 10),
                                    AddCoronaQuery(),
                                    AddShingleMatchQuery(query, 20),
                                    AddPhoneticsMatchQuery(query),
                                    AddPhraseMatchQuery(query, 10),
                                    AddFuzzyTitleAndDescriptionMatchQuery(query, 0.1),
                                    AddFuzzyPhraseMatchQuery(query, 0.1)
                                    
                                )
                                
                                .MinimumShouldMatch(1)
                            ))
                        .Negative(n => n

                            .Term(t => t
                                .Field(f => f.Title).Value("pain")
                                .Field(f => f.Description).Value("pain")
                            )
                        )
                        .NegativeBoost(0.7)
                    )

                )
                .Highlight(h =>
                    h.Fields(
                            f => f.Field(p => p.Title),
                            f => f.Field(p => p.Description).NumberOfFragments(0))
                        .PreTags(PathwaySearchResult.HighlightPreTags)
                        .PostTags(PathwaySearchResult.HighlightPostTags));
            return shouldQuery;
        }

        private Func<QueryContainerDescriptor<PathwaySearchResult>, QueryContainer> AddFuzzyPhraseMatchQuery(string query, double boostScore)
        {
            return s => s.HasChild<PathwayPhraseResult>(c =>
                c.Query(q2 =>
                    q2.Fuzzy(m =>
                        m.Field("CommonPhrase")
                            .Value(query)
                        )
                    ).Boost(boostScore)
                                          
                    .ScoreMode(ChildScoreMode.Sum)
                );
        }

        private Func<QueryContainerDescriptor<PathwaySearchResult>, QueryContainer> AddFuzzyTitleAndDescriptionMatchQuery(string query, double boostScore)
        {
            return s => s.MultiMatch(m =>
                m.Fields(f => f
                    .Field(p => p.Title, boost: 10)
                    .Field(p => p.Description, boost: 2)
                    )
                    .Operator(Operator.Or)
                    .Type(TextQueryType.MostFields)
                    .Fuzziness(Fuzziness.Auto)
                    .Slop(50)
                    .CutoffFrequency(0.001)
                    .Boost(boostScore)
                    .Query(query)
                );
        }

        private Func<QueryContainerDescriptor<PathwaySearchResult>, QueryContainer> AddPhraseMatchQuery(string query, double boostScore)
        {
            return s => s.HasChild<PathwayPhraseResult>(c => 
                c.Query(q2 => 
                    q2.Match(m => 
                        m.Field("CommonPhrase")
                            .Query(query)
                            .Boost(boostScore)
                            .Slop(50)
                        )
                    )
                    .ScoreMode(ChildScoreMode.Sum)
                );
        }

        private  Func<QueryContainerDescriptor<PathwaySearchResult>, QueryContainer> AddPhoneticsMatchQuery(string query)
        {
            return s => s.MultiMatch(m =>
                m.Fields(f => f
                    .Field(p => p.TitlePhonetic, boost: 10)
                    .Field(p => p.DescriptionPhonetic, boost: 2)
                    )
                    .Operator(Operator.Or)
                    .Type(TextQueryType.MostFields)
                    .Slop(50)
                    .CutoffFrequency(0.001)
                    .Query(query)
                );
        }

        private Func<QueryContainerDescriptor<PathwaySearchResult>, QueryContainer> AddShingleMatchQuery(string query, double boostScore)
        {
            return s => s.MultiMatch(m =>
                m.Fields(f => f
                    .Field(p => p.TitleShingles, boost: 10)
                    .Field(p => p.DescriptionShingles, boost: 2)
                    )
                    .Operator(Operator.Or)
                    .Type(TextQueryType.MostFields)
                    .Slop(50)
                    .Boost(boostScore)
                    .Query(query)
                );
        }

        private Func<QueryContainerDescriptor<PathwaySearchResult>, QueryContainer> AddTitleAndDerscriptionMatchingQuery(string query, double boostScore)
        {
            return s => s.MultiMatch(m =>
                m.Fields(f => f
                    .Field(p => p.Title, boost: 10)
                    .Field(p => p.Description, boost: 2)
                    )
                    .Operator(Operator.Or)
                    .Type(TextQueryType.MostFields)
                    .Slop(50)
                    .CutoffFrequency(0.001)
                    .Boost(boostScore)
                    .Query(query)
                );
        }

        private Func<QueryContainerDescriptor<PathwaySearchResult>, QueryContainer> AddCoronaQuery()
        {
            return s => s.MultiMatch(m =>
                m.Fields(f => f
                        .Field(p => p.Title)
                        .Field(p => p.Description)
                    )
                    .Operator(Operator.Or)
                    .Slop(50)
                    .Type(TextQueryType.MostFields)
                    .Query("Coronavirus COVID")
                    .Boost(1000)
            );
        }

        private Func<QueryContainerDescriptor<PathwaySearchResult>, QueryContainer> AddBodytags(string query, IReadOnlyCollection<IHit<BodytagResult>> bodytags)
        {
            if (bodytags.Count() > 0)
            {
                return s => s.MultiMatch(m =>
                    m.Fields(f => f
                            .Field(p => p.Title)
                            .Field(p => p.Description)
                        )
                        .Operator(Operator.Or)
                        .Type(TextQueryType.MostFields)
                        .Query(string.Join(" ", bodytags.Select(b => b.Source.Tag)))
                );
            }
            return s => null;

        }

        private SearchDescriptor<PathwaySearchResult> AddAgeGenderFilters(
            SearchDescriptor<PathwaySearchResult> searchDescriptor, string gender, string ageGroup)
        {
            return searchDescriptor.PostFilter(pf =>
                pf.Bool(b => b
                    .Must(
                        m => m.Match(p => p.Field(f => f.Gender).Query(gender)),
                        m => m.Match(p => p.Field(f => f.AgeGroup).Query(ageGroup))
                    )
                    ));
        }
    }

    public interface IPathwaySearchService
    {
        Task<List<PathwaySearchResult>> FindResults(string query, bool highlight, bool score);
        Task<List<PathwaySearchResult>> FindResults(string query, string gender, string ageGroup, bool highlight, bool score);
    }
    
}
