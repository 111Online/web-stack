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

        public async Task<List<PathwaySearchResult>> FindResults(string query)
        {
            var res = await _elastic.SearchAsync<PathwaySearchResult>(s =>
                BuildPathwaysTextQuery(s.Index("pathways"), query)
                );

            return res.Hits.Select(h => h.Source).ToList(); 
        }

        public async Task<List<PathwaySearchResult>> FindResults(string query, string gender, string ageGroup)
        {
            var res = await _elastic.SearchAsync<PathwaySearchResult>(s =>
                AddAgeGenderFilters(BuildPathwaysTextQuery(s.Index("pathways"), query), gender, ageGroup));

            return res.Hits.Select(h => h.Source).ToList();
            ;
        }

        private SearchDescriptor<PathwaySearchResult> BuildPathwaysTextQuery(
            SearchDescriptor<PathwaySearchResult> searchDescriptor, string query)
        {
            return searchDescriptor.Query(q =>
                q.MultiMatch(m =>
                    m.Fields(f => f
                        .Field(p => p.Title, boost: 6)
                        .Field(p => p.Description, boost: 2)
                        .Field(p => p.PathwayTitle)
                        ).Operator(Operator.Or)
                        .Type(TextQueryType.MostFields)
                        .Fuzziness(Fuzziness.Auto)
                        .Query(query)
                    )
                );
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
        Task<List<PathwaySearchResult>> FindResults(string query);
        Task<List<PathwaySearchResult>> FindResults(string query, string gender, string ageGroup);
    }
    
}
