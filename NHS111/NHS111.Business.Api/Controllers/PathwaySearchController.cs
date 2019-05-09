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
        private readonly ISearchResultFilter _searchResultFilter;
        public PathwaySearchController(IPathwaySearchService pathwaySearchService, ISearchResultFilter searchResultFilter)
        {
            _pathwaySearchService = pathwaySearchService;
            _searchResultFilter = searchResultFilter;

        }


        [Route("pathwaysearch/{query}")]
        public async Task<List<PathwaySearchResult>> Get(string query, [FromUri] bool highlight = false, [FromUri] bool score = false)
        {
            var results =  await _pathwaySearchService.FindResults(query, highlight, score);
            var filteredresults = await _searchResultFilter.Filter(results, new Dictionary<string, string>());
            return filteredresults.ToList();
        }

        [Route("pathwaysearch/{gender}/{ageGroup}")]
        [HttpPost]
        public async Task<List<PathwaySearchResult>> Get(string gender, string ageGroup, [FromBody] string query, [FromUri] bool highlight = false, [FromUri] bool score = false)
        {
            var results = await _pathwaySearchService.FindResults(query, gender, ageGroup, highlight, score);
            var filteredresults = await _searchResultFilter.Filter(results, new Dictionary<string, string>());
            return filteredresults.ToList();
        }
    }

  
}
