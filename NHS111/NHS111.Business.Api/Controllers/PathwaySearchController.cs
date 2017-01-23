using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Business.Services;
using NHS111.Models.Models.Business.PathwaySearch;
using NHS111.Utils.Attributes;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Api.Controllers
{
    [LogHandleErrorForApi]
    public class PathwaySearchController : ApiController
    {
        private readonly IPathwaySearchService _pathwaySearchService;

        public PathwaySearchController(IPathwaySearchService pathwaySearchService)
        {
            _pathwaySearchService = pathwaySearchService;
        }


        [Route("pathwaysearch/{query}")]
        public async Task<List<PathwaySearchResult>> Get(string query)
        {
            var results =  await _pathwaySearchService.FindResults(query);
            return results;
        }

        [Route("pathwaysearch/{gender}/{ageGroup}/{query}")]
        public async Task<List<PathwaySearchResult>> Get(string gender, string ageGroup, string query)
        {
            var results = await _pathwaySearchService.FindResults(query, gender, ageGroup);
            return results;
        }
    }
}
