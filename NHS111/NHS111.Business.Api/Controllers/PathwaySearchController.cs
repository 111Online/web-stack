using NHS111.Business.Services;
using NHS111.Models.Models.Business.PathwaySearch;
using NHS111.Utils.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

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
            var results = await _pathwaySearchService.FindResults(query, highlight, score);
            return results;
        }

        [Route("pathwaysearch/{gender}/{ageGroup}")]
        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<List<PathwaySearchResult>> Get(string gender, string ageGroup, [FromBody] SearchRequest request, [FromUri] bool highlight = false, [FromUri] bool score = false)
        {
            var results = await _pathwaySearchService.FindResults(request.Query, gender, ageGroup, highlight, score);

            if (request.Postcode != null)
            {
                var filteredresults = await _searchResultFilter.Filter(results, new Dictionary<string, string>() { { "postcode", request.Postcode } });
                return filteredresults.ToList();
            }

            return results;
        }
    }

    // This is used as a model purely for the search POST as multiple [FromBody] do not work
    public class SearchRequest
    {
        public string Query { get; set; }

        public string Postcode { get; set; }
    }


}
