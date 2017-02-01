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
            var res = await _elastic.SearchAsync<PathwaySearchResult>(s =>
                    BuildPathwaysTextQuery(s.Index("pathways"), query)
                );

            var highlightedResults = BuildHighlights(res.Hits, highlight);

            return BuildScoredResults(highlightedResults, res.Hits, score).ToList();
        }

        public async Task<List<PathwaySearchResult>> FindResults(string query, string gender, string ageGroup, bool highlight, bool score)
        {
            var res = await _elastic.SearchAsync<PathwaySearchResult>(s =>
                AddAgeGenderFilters(BuildPathwaysTextQuery(s.Index("pathways"), query), gender, ageGroup));

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
                                hit.Source.Title = highlight.Value.Highlights.ToList();
                                break;
                        }

                    }
                }
            }

            return hits.Select(h => h.Source);
        }

        private SearchDescriptor<PathwaySearchResult> BuildPathwaysTextQuery(
            SearchDescriptor<PathwaySearchResult> searchDescriptor, string query)
        {
            // boost exact matches on parent and child documents
            // then fuzzy match
            // slop value scores higher where more than one words from the query are found in a document (I THINK!)
            var shouldQuery = searchDescriptor.Query(q => q
                            .Bool(b => b
                                .Should
                                    (
                                        s => s.MultiMatch(m =>
                                            m.Fields(f => f
                                                    .Field(p => p.Title, boost: 6)
                                                    .Field(p => p.Description, boost: 2)
                                                )
                                                .Operator(Operator.Or)
                                                .Type(TextQueryType.MostFields)
                                                .Slop(50)
                                                .Boost(10)
                                                .Query(query)
                                            ),
                                        s => s.HasChild<PathwayPhraseResult>(c => 
                                            c.Query(q2 => 
                                                q2.Match(m => 
                                                    m.Field("CommonPhrase")
                                                        .Query(query)
                                                        .Boost(10)
                                                        .Slop(50)
                                                        )
                                                    )
                                                    .ScoreMode(ChildScoreMode.Sum)
                                            ),
                                        s => s.MultiMatch(m =>
                                            m.Fields(f => f
                                                    .Field(p => p.Title, boost: 6)
                                                    .Field(p => p.Description, boost: 2)
                                                )
                                                .Operator(Operator.Or)
                                                .Type(TextQueryType.MostFields)
                                                .Fuzziness(Fuzziness.Auto)
                                                .Slop(50)
                                                .Query(query)
                                            ),
                                        s => s.HasChild<PathwayPhraseResult>(c =>
                                            c.Query(q2 =>
                                                q2.Fuzzy(m =>
                                                    m.Field("CommonPhrase")
                                                        .Value(query)
                                                    )
                                                )
                                                .ScoreMode(ChildScoreMode.Sum)
                                            )
                                    )
                               .MinimumShouldMatch(1)
                               ))
                    .Highlight(h => 
                        h.Fields(
                            f => f.Field(p => p.Title),
                            f => f.Field(p => p.Description).NumberOfFragments(0))
                .PreTags("<em class='highlight-term'>")
                .PostTags("</em>"));
            ;

            return shouldQuery;
        }

        private SearchDescriptor<PathwaySearchResult> AddAgeGenderFilters(
            SearchDescriptor<PathwaySearchResult> searchDescriptor, string gender, string ageGroup)
        {
            return searchDescriptor.PostFilter(pf =>
                pf.Bool(b => b
                    .Must(
                        m => m.Term(p => p.Gender, gender),
                        m => m.Term(p => p.AgeGroup, ageGroup)
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
