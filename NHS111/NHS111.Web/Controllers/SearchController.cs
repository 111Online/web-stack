using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Business.PathwaySearch;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Web.Helpers;
using RestSharp;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;

namespace NHS111.Web.Controllers
{
    public class SearchController : Controller
    {
        public const int MAX_SEARCH_RESULTS = 10;

        public SearchController(IConfiguration configuration, IUserZoomDataBuilder userZoomDataBuilder, IRestClient restClientBusinessApi)
        {
            _configuration = configuration;
            _userZoomDataBuilder = userZoomDataBuilder;
            _restClientBusinessApi = restClientBusinessApi;
        }

        [HttpPost]
        public ActionResult Search(JourneyViewModel model)
        {
            if (!ModelState.IsValidField("UserInfo.Demography.Gender") || !ModelState.IsValidField("UserInfo.Demography.Age"))
            {
                _userZoomDataBuilder.SetFieldsForDemographics(model);
                return View("~\\Views\\Question\\Gender.cshtml", model);
            }

            var startOfJourney = new SearchJourneyViewModel
            {
                SessionId = model.SessionId,
                UserInfo = new UserInfo
                {
                    Demography = model.UserInfo.Demography,
                    CurrentAddress = model.UserInfo.CurrentAddress
                },
                FilterServices = model.FilterServices,
                Campaign = model.Campaign,
                Source = model.Source
            };

            _userZoomDataBuilder.SetFieldsForSearch(startOfJourney);

            return View(startOfJourney);

        }

        [HttpPost]
        public async Task<ActionResult> SearchResults(SearchJourneyViewModel model)
        {
            if (!ModelState.IsValidField("SanitisedSearchTerm")) return View("Search", model);
            
            var ageGroup = new AgeCategory(model.UserInfo.Demography.Age);
            model.EntrySearchTerm = model.SanitisedSearchTerm;

            _userZoomDataBuilder.SetFieldsForSearchResults(model);

            var requestPath = _configuration.GetBusinessApiPathwaySearchUrl(model.UserInfo.Demography.Gender, ageGroup.Value, true);

            var request = new RestRequest(requestPath, Method.POST);
            request.AddJsonBody(Uri.EscapeDataString(model.SanitisedSearchTerm.Trim()));

            var response = await _restClientBusinessApi.ExecuteTaskAsync<List<SearchResultViewModel>>(request);

            model.Results = response.Data
                .Take(MAX_SEARCH_RESULTS)
                .Select(r => Transform(r, model.SanitisedSearchTerm.Trim()));

            if (!model.Results.Any())
            {

                var encryptedTopicsQueryStringValues = KeyValueEncryptor.EncryptedKeys(model);
                    
                return RedirectToRoute("CatergoriesUrl",
                    new
                    {
                        gender = model.UserInfo.Demography.Gender,
                        age = model.UserInfo.Demography.Age.ToString(),
                        args = encryptedTopicsQueryStringValues
                    });
            }

            return View(model);
        }

        [HttpGet]
        [Route("{gender}/{age}/Topics", Name = "CatergoriesUrl")]
        public async Task<ActionResult> Categories(string gender, int age, string args)
        {
            var decryptedArgs = new QueryStringEncryptor(args);

            var ageGenderViewModel = new AgeGenderViewModel { Gender = gender, Age = age };
            var topicsContainingStartingPathways = await GetAllTopics(ageGenderViewModel);
            var model = new SearchJourneyViewModel
            {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                UserInfo = new UserInfo
                {
                    Demography = ageGenderViewModel,
                    CurrentAddress = new FindServicesAddressViewModel() { Postcode = decryptedArgs["postcode"] }
                },
                AllTopics = topicsContainingStartingPathways,
                FilterServices = bool.Parse(decryptedArgs["filterServices"]),
                SanitisedSearchTerm = decryptedArgs["searchTerm"],
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"]
            };

            _userZoomDataBuilder.SetFieldsForSearchResults(model);

            return View(model);

        }

        private async Task<IEnumerable<CategoryWithPathways>> GetAllTopics(AgeGenderViewModel model)
        {
            var url = _configuration.GetBusinessApiGetCategoriesWithPathwaysGenderAge(model.Gender,
                model.Age, true);
            var response = await
                _restClientBusinessApi.ExecuteTaskAsync<List<CategoryWithPathways>>(CreateJsonRequest(url, Method.GET));


            var allTopics = response.Data;
            var topicsContainingStartingPathways =
                allTopics.Where(
                    c =>
                        c.Pathways.Any(p => p.Pathway.StartingPathway) ||
                        c.SubCategories.Any(sc => sc.Pathways.Any(p => p.Pathway.StartingPathway)));
            return topicsContainingStartingPathways;
        }

        private SearchResultViewModel Transform(SearchResultViewModel result, string searchTerm)
        {
            result.Description += ".";
            result.Description = result.Description.Replace("\\n\\n", ". ");
            result.Description = result.Description.Replace(" . ", ". ");
            result.Description = result.Description.Replace("..", ".");

            SortTitlesByRelevancy(result, searchTerm);

            return result;
        }

        private void SortTitlesByRelevancy(SearchResultViewModel result, string searchTerm)
        {
            if (result.DisplayTitle == null)
                return;
            var lowerTerm = searchTerm.ToLower();
            for (var i = 0; i < result.DisplayTitle.Count; i++)
            {
                var title = result.DisplayTitle[i];
                if (!PathwaySearchResult.StripHighlightMarkup(title).ToLower().Contains(lowerTerm))
                    continue;
                result.DisplayTitle.RemoveAt(i);
                result.DisplayTitle.Insert(0, title);
            }
        }

        private RestRequest CreateJsonRequest(string url, Method method)
        {
            var request = new RestRequest(url, method);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            return request;
        }

        private readonly IConfiguration _configuration;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;
        private readonly IRestClient _restClientBusinessApi;
    }
}