using NHS111.Models.Models.Business.PathwaySearch;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Utils.RestTools;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NHS111.Web.Controllers
{
    using System.Web.Routing;

    public class SearchController : Controller
    {
        public const int MAX_SEARCH_RESULTS = 10;

        public SearchController(IConfiguration configuration, IUserZoomDataBuilder userZoomDataBuilder, IRestClient restClientBusinessApi, IJustToBeSafeFirstViewModelBuilder jtbsViewModelBuilder)
        {
            _configuration = configuration;
            _userZoomDataBuilder = userZoomDataBuilder;
            _restClientBusinessApi = restClientBusinessApi;
            _jtbsViewModelBuilder = jtbsViewModelBuilder;
        }

        [HttpPost]
        public async Task<ActionResult> Search(JourneyViewModel model)
        {
            if (!ModelState.IsValidField("UserInfo.Demography.Gender") || !ModelState.IsValidField("UserInfo.Demography.Age"))
            {
                _userZoomDataBuilder.SetFieldsForDemographics(model);
                return View("~\\Views\\Question\\Gender.cshtml", model);
            }


            if (model.PathwayNo != null)
            {
                var pathwayMetadata = await _restClientBusinessApi.ExecuteTaskAsync<PathwayMetaData>(
                            new RestRequest(_configuration.GetBusinessApiPathwayMetadataUrl(model.PathwayNo),
                                Method.GET));
                var digitalTitle = pathwayMetadata.Data.DigitalTitle;

                var searchJourneyViewModel = new SearchJourneyViewModel()
                {
                    SessionId = model.SessionId,
                    PathwayNo = model.PathwayNo.ToUpper(),
                    DigitalTitle = digitalTitle,
                    CurrentPostcode = model.CurrentPostcode,
                    UserInfo = model.UserInfo,
                    FilterServices = model.FilterServices,
                    Campaign = model.Campaign,
                    Source = model.Source,
                    IsCustomJourney = model.IsCustomJourney
                };

                return RedirectToAction("FirstQuestion", "JustToBeSafe", new RouteValueDictionary {
                    { "pathwayNumber", searchJourneyViewModel.PathwayNo },
                    { "gender", searchJourneyViewModel.UserInfo.Demography.Gender},
                    { "age", searchJourneyViewModel.UserInfo.Demography.Age},
                    { "args", KeyValueEncryptor.EncryptedKeys(searchJourneyViewModel)} });
            }

            var startOfJourney = new SearchJourneyViewModel
            {
                SessionId = model.SessionId,
                CurrentPostcode = model.CurrentPostcode,
                UserInfo = new UserInfo
                {
                    Demography = model.UserInfo.Demography,
                    CurrentAddress = model.UserInfo.CurrentAddress
                },
                FilterServices = model.FilterServices,
                Campaign = model.Campaign,
                Source = model.Source,
                IsCustomJourney = model.IsCustomJourney
            };

            _userZoomDataBuilder.SetFieldsForSearch(startOfJourney);

            return View(startOfJourney);

        }


        [HttpGet]
        [Route("{gender}/{age}/Search", Name = "SearchUrl")]
        public ActionResult SearchDirect(string gender, int age, string args)
        {
            var decryptedArgs = new QueryStringEncryptor(args);
            var ageGenderViewModel = new AgeGenderViewModel { Gender = gender, Age = age };
            var startOfJourney = new SearchJourneyViewModel
            {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                CurrentPostcode = decryptedArgs["postcode"],
                UserInfo = new UserInfo
                {
                    Demography = ageGenderViewModel,
                },
                FilterServices = bool.Parse(decryptedArgs["filterServices"]),
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                IsCustomJourney = bool.Parse(decryptedArgs["IsCustomJourney"])
            };

            _userZoomDataBuilder.SetFieldsForSearch(startOfJourney);

            return View("~\\Views\\Search\\Search.cshtml", startOfJourney);

        }

        [HttpPost]
        public async Task<ActionResult> SearchResults(SearchJourneyViewModel model)
        {
            if (!ModelState.IsValidField("SanitisedSearchTerm")) return View("~\\Views\\Search\\NoResults.cshtml", model);

            var ageGroup = new AgeCategory(model.UserInfo.Demography.Age);
            model.EntrySearchTerm = model.SanitisedSearchTerm;

            _userZoomDataBuilder.SetFieldsForSearchResults(model);

            var requestPath = _configuration.GetBusinessApiPathwaySearchUrl(model.UserInfo.Demography.Gender, ageGroup.Value, true);

            var request = new RestRequest(requestPath, Method.POST);
            if (model.SanitisedSearchTerm == null)
                return View("~\\Views\\Search\\NoResults.cshtml", model);

            request.AddJsonBody(new { query = Uri.EscapeDataString(model.SanitisedSearchTerm.Trim()), postcode = Uri.EscapeDataString(model.CurrentPostcode) });

            var response = await _restClientBusinessApi.ExecuteTaskAsync<List<SearchResultViewModel>>(request);

            model.Results = response.Data
                .Take(MAX_SEARCH_RESULTS)
                .Select(r => Transform(r, model.SanitisedSearchTerm.Trim()));

            if (!model.Results.Any())
                return View("~\\Views\\Search\\NoResults.cshtml", model);

            return View(model);
        }

        [HttpGet]
        [Route("{gender}/{age}/Categories", Name = "CatergoriesUrl")]
        public async Task<ActionResult> Categories(string gender, int age, string args, bool hasResults = false)
        {
            var decryptedArgs = new QueryStringEncryptor(args);

            var ageGenderViewModel = new AgeGenderViewModel { Gender = gender, Age = age };
            var categoriesContainingStartingPathways = await GetAllCategories(ageGenderViewModel, decryptedArgs["postcode"]);
            var model = new SearchJourneyViewModel
            {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                CurrentPostcode = decryptedArgs["postcode"],
                UserInfo = new UserInfo
                {
                    Demography = ageGenderViewModel,
                },
                Categories = categoriesContainingStartingPathways,
                FilterServices = bool.Parse(decryptedArgs["filterServices"]),
                SanitisedSearchTerm = decryptedArgs["searchTerm"],
                EntrySearchTerm = decryptedArgs["searchTerm"],
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                HasResults = hasResults,
                IsCustomJourney = bool.Parse(decryptedArgs["IsCustomJourney"])
            };

            _userZoomDataBuilder.SetFieldsForSearchResults(model);

            return View(model);

        }

        [HttpGet]
        [Route("{gender}/{age}/Category/{category}", Name = "CatergoryUrl")]
        public async Task<ActionResult> Category(string gender, int age, string category, string args,
            bool hasResults = false)
        {
            var decryptedArgs = new QueryStringEncryptor(args);

            var ageGenderViewModel = new AgeGenderViewModel { Gender = gender, Age = age };
            var categoriesContainingStartingPathways = await GetAllCategories(ageGenderViewModel, decryptedArgs["postcode"]);
            var model = new SearchJourneyViewModel
            {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                CurrentPostcode = decryptedArgs["postcode"],
                UserInfo = new UserInfo
                {
                    Demography = ageGenderViewModel,
                },
                Categories = categoriesContainingStartingPathways.Where(c => c.Category.Title == category),
                FilterServices = bool.Parse(decryptedArgs["filterServices"]),
                SanitisedSearchTerm = decryptedArgs["searchTerm"],
                EntrySearchTerm = decryptedArgs["searchTerm"],
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                HasResults = hasResults,
                IsCustomJourney = bool.Parse(decryptedArgs["IsCustomJourney"])
            };

            _userZoomDataBuilder.SetFieldsForSearchResults(model);

            return View("Pathways", model);
        }

        [HttpGet]
        [Route("{gender}/{age}/Pathways", Name = "PathwaysUrl")]
        public async Task<ActionResult> Pathways(string gender, int age, string args, bool hasResults = false)
        {
            var decryptedArgs = new QueryStringEncryptor(args);

            var ageGenderViewModel = new AgeGenderViewModel { Gender = gender, Age = age };
            var categoriesContainingStartingPathways = await GetAllCategories(ageGenderViewModel, decryptedArgs["postcode"]);
            var rootCategory = new CategoryWithPathways
            {
                Category = new Category { Title = "All topics" },
                Pathways = FlattenCategories(categoriesContainingStartingPathways)
            };
            var model = new SearchJourneyViewModel
            {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                CurrentPostcode = decryptedArgs["postcode"],
                UserInfo = new UserInfo
                {
                    Demography = ageGenderViewModel,
                },
                Categories = new List<CategoryWithPathways> { rootCategory },
                FilterServices = bool.Parse(decryptedArgs["filterServices"]),
                SanitisedSearchTerm = decryptedArgs["searchTerm"],
                EntrySearchTerm = decryptedArgs["searchTerm"],
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                HasResults = hasResults,
                IsCustomJourney = bool.Parse(decryptedArgs["IsCustomJourney"])
            };

            _userZoomDataBuilder.SetFieldsForSearchResults(model);

            return View(model);

        }

        private IEnumerable<PathwayWithDescription> FlattenCategories(IEnumerable<CategoryWithPathways> categories, List<PathwayWithDescription> results = null)
        {
            if (results == null)
                results = new List<PathwayWithDescription>();

            if (categories == null)
                return results;

            foreach (var category in categories)
            {
                results.AddRange(category.Pathways.Where(p => results.All(r => r.PathwayData.DigitalTitle != p.PathwayData.DigitalTitle)));
                FlattenCategories(category.SubCategories, results);
            }

            return results;
        }

        private async Task<IEnumerable<CategoryWithPathways>> GetAllCategories(AgeGenderViewModel model, string postcode)
        {
            var requestPath = _configuration.GetBusinessApiGetCategoriesWithPathwaysGenderAge(model.Gender,
                model.Age, true);

            var request = new RestRequest(requestPath, Method.POST);
            request.AddJsonBody(postcode);

            var response = await _restClientBusinessApi.ExecuteTaskAsync<List<CategoryWithPathways>>(request);


            var allCategories = response.Data;
            var categoriesContainingStartingPathways =
                allCategories.Where(
                    c =>
                        c.Pathways.Any(p => p.Pathway.StartingPathway) ||
                        c.SubCategories.Any(sc => sc.Pathways.Any(p => p.Pathway.StartingPathway)));
            return categoriesContainingStartingPathways;
        }


        private async Task<IEnumerable<Pathway>> GetAllPathways(AgeGenderViewModel model)
        {
            var url = _configuration.GetBusinessApiGetPathwaysGenderAge(model.Gender, model.Age);
            var response = await _restClientBusinessApi.ExecuteTaskAsync<List<Pathway>>(new JsonRestRequest(url, Method.GET));

            return response.Data;
        }

        private SearchResultViewModel Transform(SearchResultViewModel result, string searchTerm)
        {
            result.Description += ".";
            result.Description = result.Description.Replace("\\n\\n", ". ");
            result.Description = result.Description.Replace(" . ", ". ");
            result.Description = result.Description.Replace("..", ".");

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

        private readonly IConfiguration _configuration;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;
        private readonly IRestClient _restClientBusinessApi;
        private readonly IJustToBeSafeFirstViewModelBuilder _jtbsViewModelBuilder;
    }
}