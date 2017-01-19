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
            _elastic = new ElasticClient(_configuration.GetElasticClientSettings().DisableDirectStreaming().OnRequestCompleted(details =>
            {
                Debug.WriteLine("### ES REQEUST ###");
                if (details.RequestBodyInBytes != null) Debug.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                Debug.WriteLine("### ES RESPONSE ###");
                if (details.ResponseBodyInBytes != null) Debug.WriteLine(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
            }));
        }

        public PathwaySearchService(IElasticClient elastic)
        {
            _elastic = elastic;
        }

        public async Task<List<PathwaySearchResult>> FindResults(string query)
        {
           var res = await  _elastic.SearchAsync<PathwaySearchResult>(s =>
                s.Index("pathways").Query(q =>
                    q.MultiMatch(m =>
                        m.Fields(f =>f
                            .Field(p => p.Title)
                            .Field(p => p.Description)
                                ).Operator(Operator.Or)
                                .Type(TextQueryType.MostFields)
                                .Fuzziness(Fuzziness.Auto)
                                .Query(query)
                                )
                        )
            );

            return res.Hits.Select(h => h.Source).ToList(); ;
        }
    }

    public interface IPathwaySearchService
    {
        Task<List<PathwaySearchResult>> FindResults(string query);
    }
}
