using NHS111.Models.Models.Business.PathwaySearch;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Utils.RestTools;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using NHS111.Features;
using NHS111.Models.Models.Web.Enums;
using NHS111.Web.Presentation.Logging;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Controllers
{
    using System.Web.Routing;

    public class SearchController : Controller
    {
        public const int MAX_SEARCH_RESULTS = 10;
        private const string EmergencyPrescriptionPathwayNumber = "PW1827";

        public SearchController(
            IConfiguration configuration,
            IUserZoomDataBuilder userZoomDataBuilder,
            ILoggingRestClient restClientBusinessApi,
            IAuditLogger auditLogger
            )
        {
            _configuration = configuration;
            _userZoomDataBuilder = userZoomDataBuilder;
            _restClientBusinessApi = restClientBusinessApi;
            _auditLogger = auditLogger;
        }

        [HttpPost]
        public async Task<ActionResult> Search(SearchJourneyViewModel model)
        {
            if (!ModelState.IsValidField("UserInfo.Demography.Gender") || !ModelState.IsValidField("UserInfo.Demography.Age"))
            {
                _userZoomDataBuilder.SetFieldsForDemographics(model);
                return View("~\\Views\\Question\\Gender.cshtml", model);
            }

            if (model.IsCovidJourney)
            {
                return RedirectToGuidedSelection(SearchReservedCovidTerms.SearchTerms.First(), model, true);
            }

            if (model.PathwayNo != null && model.PathwayNo.Equals("none"))
            {
                _auditLogger.LogEvent(model, EventType.GuidedSelection, "None", "/GuidedSelection");
                return RedirectToExplainer(model);
            } 
            
            if (model.PathwayNo != null)
            {
                model.ViaGuidedSelection = !model.PathwayNo.ToUpper().Equals(EmergencyPrescriptionPathwayNumber);
                return await RedirectToFirstTriageQuestion(model).ConfigureAwait(false);
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
                Source = model.Source
            };

            _userZoomDataBuilder.SetFieldsForSearch(startOfJourney);

            return View(startOfJourney);
        }

        private ActionResult RedirectToGuidedSelection(string searchTerm, JourneyViewModel model, bool isCovidJourney = false)
        {
            var searchJourneyViewModel = new SearchJourneyViewModel()
            {
                SessionId = model.SessionId,
                CurrentPostcode = model.CurrentPostcode,
                UserInfo = model.UserInfo,
                FilterServices = model.FilterServices,
                Campaign = model.Campaign,
                Source = model.Source,
                SanitisedSearchTerm = searchTerm,
                IsCovidJourney = isCovidJourney
            };

            model.ViaGuidedSelection = false;

            return RedirectToAction("GuidedSelection", new RouteValueDictionary {
                { "gender", model.UserInfo.Demography.Gender},
                { "age", model.UserInfo.Demography.Age},
                { "args", KeyValueEncryptor.EncryptedKeys(searchJourneyViewModel)} });
        }

        private ActionResult RedirectToExplainer(SearchJourneyViewModel model)
        {
            var searchJourneyViewModel = new SearchJourneyViewModel()
            {
                SessionId = model.SessionId,
                CurrentPostcode = model.CurrentPostcode,
                UserInfo = model.UserInfo,
                FilterServices = model.FilterServices,
                Campaign = model.Campaign,
                Source = model.Source,
                SanitisedSearchTerm = model.SanitisedSearchTerm
            };

            return RedirectToAction("Explainer", new RouteValueDictionary {
                { "gender", model.UserInfo.Demography.Gender},
                { "age", model.UserInfo.Demography.Age},
                { "args", KeyValueEncryptor.EncryptedKeys(searchJourneyViewModel)} });
        }

        private async Task<ActionResult> RedirectToFirstTriageQuestion(JourneyViewModel model)
        {
            var pathwayMetadata = await _restClientBusinessApi.ExecuteAsync<PathwayMetaData>(
                new RestRequest(_configuration.GetBusinessApiPathwayMetadataUrl(model.PathwayNo),
                    Method.GET)).ConfigureAwait(false);
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
                ViaGuidedSelection = model.ViaGuidedSelection
            };

            return RedirectToAction("FirstQuestion", "JustToBeSafe", new RouteValueDictionary {
                { "pathwayNumber", searchJourneyViewModel.PathwayNo },
                { "gender", searchJourneyViewModel.UserInfo.Demography.Gender},
                { "age", searchJourneyViewModel.UserInfo.Demography.Age},
                { "args", KeyValueEncryptor.EncryptedKeys(searchJourneyViewModel)} });
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
                Source = decryptedArgs["source"]
            };

            _userZoomDataBuilder.SetFieldsForSearch(startOfJourney);

            return View("~\\Views\\Search\\Search.cshtml", startOfJourney);

        }

        [HttpPost]
        public async Task<ActionResult> SearchResults(SearchJourneyViewModel model)
        {
            //Could combine these no results?
            if (!ModelState.IsValidField("SanitisedSearchTerm")) return NoResults(model);

            model.EntrySearchTerm = model.SanitisedSearchTerm;

            if (model.SanitisedSearchTerm == null) return NoResults(model);

            if (FeatureRouter.CovidSearchRedirect(HttpContext.Request.Params) && model.IsReservedCovidSearchTerm)
                return RedirectToGuidedSelection(model.SanitisedSearchTerm, model);

            model.ViaGuidedSelection = false;
            return await SearchResultsView(model).ConfigureAwait(false);
        }

        public ViewResult Explainer(string gender, int age, string args)
        {
            var decryptedArgs = new QueryStringEncryptor(args);
            var ageGenderViewModel = new AgeGenderViewModel { Gender = gender, Age = age };

            var model = new SearchJourneyViewModel
            {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                CurrentPostcode = decryptedArgs["postcode"],
                UserInfo = new UserInfo
                {
                    Demography = ageGenderViewModel,
                },
                FilterServices = bool.Parse(decryptedArgs["filterServices"]),
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"]
            };

            return View("~\\Views\\Search\\Coronavirus_Explainer.cshtml", model);
        }

        public async Task<ViewResult> GuidedSelection(string gender, int age, string args)
        {
            var decryptedArgs = new QueryStringEncryptor(args);
            var ageGenderViewModel = new AgeGenderViewModel { Gender = gender, Age = age };

            var model = new SearchJourneyViewModel
            {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                CurrentPostcode = decryptedArgs["postcode"],
                UserInfo = new UserInfo
                {
                    Demography = ageGenderViewModel,
                },
                SanitisedSearchTerm = decryptedArgs["searchTerm"],
                FilterServices = bool.Parse(decryptedArgs["filterServices"]),
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                IsCovidJourney = bool.Parse(decryptedArgs["isCovidjourney"])
            };

            var ageGroup = new AgeCategory(model.UserInfo.Demography.Age);
            var requestPath = _configuration.GetBusinessApiGuidedPathwaySearchUrl(model.UserInfo.Demography.Gender, ageGroup.Value, true);

            var request = new RestRequest(requestPath, Method.POST);
            request.AddJsonBody(new { query = Uri.EscapeDataString(model.SanitisedSearchTerm.Trim().ToLower()) });

            var response = await _restClientBusinessApi.ExecuteAsync<List<GuidedSearchResultViewModel>>(request).ConfigureAwait(false);

            var guidedModel = Mapper.Map<GuidedSearchJourneyViewModel>(model);
            guidedModel.GuidedResults = response.Data;

            return !guidedModel.GuidedResults.Any() ? NoResults(model) : View("~\\Views\\Search\\GuidedSelection.cshtml", guidedModel);
        }

        [HttpGet]
        [Route("{gender}/{age}/SearchResultsDirect")]
        public async Task<ActionResult> SearchResultsDirect(string gender, int age, string searchTerm, string args)
        {
            var decryptedArgs = new QueryStringEncryptor(args);
            var ageGenderViewModel = new AgeGenderViewModel { Gender = gender, Age = age };

            var model = new SearchJourneyViewModel
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
                SanitisedSearchTerm = searchTerm
            };

            return await SearchResults(model);
        }

        [HttpGet]
        [Route("{gender}/{age}/Categories", Name = "CatergoriesUrl")]
        public async Task<ActionResult> Categories(string gender, int age, string args, bool hasResults = false)
        {
            var decryptedArgs = new QueryStringEncryptor(args);

            var ageGenderViewModel = new AgeGenderViewModel { Gender = gender, Age = age };
            var categoriesContainingStartingPathways = await GetAllCategories(ageGenderViewModel, decryptedArgs["postcode"]).ConfigureAwait(false);
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
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                HasResults = hasResults
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
            var categoriesContainingStartingPathways = await GetAllCategories(ageGenderViewModel, decryptedArgs["postcode"]).ConfigureAwait(false);
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
                EntrySearchTerm = decryptedArgs["entrySearchTerm"],
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                HasResults = hasResults
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
            var categoriesContainingStartingPathways = await GetAllCategories(ageGenderViewModel, decryptedArgs["postcode"]).ConfigureAwait(false);
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
                EntrySearchTerm = decryptedArgs["entrySearchTerm"],
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                HasResults = hasResults
            };

            _userZoomDataBuilder.SetFieldsForSearchResults(model);

            return View(model);
        }

        private ViewResult NoResults(SearchJourneyViewModel model)
        {
            return View("~\\Views\\Search\\NoResults.cshtml", model);
        }

        private async Task<ViewResult> SearchResultsView(SearchJourneyViewModel model)
        {
            var ageGroup = new AgeCategory(model.UserInfo.Demography.Age);

            _userZoomDataBuilder.SetFieldsForSearchResults(model);

            var requestPath = _configuration.GetBusinessApiPathwaySearchUrl(model.UserInfo.Demography.Gender, ageGroup.Value, true);

            var request = new RestRequest(requestPath, Method.POST);


            request.AddJsonBody(new { query = Uri.EscapeDataString(model.SanitisedSearchTerm.Trim()), postcode = Uri.EscapeDataString(model.CurrentPostcode) });

            var response = await _restClientBusinessApi.ExecuteAsync<List<SearchResultViewModel>>(request).ConfigureAwait(false);

            model.Results = response.Data
                .Take(MAX_SEARCH_RESULTS)
                .Select(Transform);

            return !model.Results.Any() ? NoResults(model) : View("~\\Views\\Search\\SearchResults.cshtml", model);
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

            var response = await _restClientBusinessApi.ExecuteAsync<List<CategoryWithPathways>>(request).ConfigureAwait(false);


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
            var response = await _restClientBusinessApi.ExecuteAsync<List<Pathway>>(new JsonRestRequest(url, Method.GET)).ConfigureAwait(false);

            return response.Data;
        }

        private SearchResultViewModel Transform(SearchResultViewModel result)
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
        private readonly ILoggingRestClient _restClientBusinessApi;
        private readonly IAuditLogger _auditLogger;
    }
}