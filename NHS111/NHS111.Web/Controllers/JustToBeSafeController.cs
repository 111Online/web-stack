using AutoMapper;
using Newtonsoft.Json;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;
using NHS111.Utils.RestTools;
using NHS111.Web.Helpers;
using NHS111.Web.Presentation.Builders;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class JustToBeSafeController : Controller
    {
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
        private readonly IJustToBeSafeViewModelBuilder _justToBeSafeViewModelBuilder;
        private readonly IConfiguration _configuration;
        private readonly ILoggingRestClient _restClientBusinessApi;

        private const string GuidedSelectionPathwayNumber = "PW1851";

        public JustToBeSafeController(
            IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder, 
            IJustToBeSafeViewModelBuilder justToBeSafeViewModelBuilder, 
            IConfiguration configuration,
            ILoggingRestClient restClientBusinessApi
)
        {
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _justToBeSafeViewModelBuilder = justToBeSafeViewModelBuilder;
            _configuration = configuration;
            _restClientBusinessApi = restClientBusinessApi;
        }

        [HttpPost]
        public async Task<ActionResult> JustToBeSafeFirst(JustToBeSafeViewModel model)
        {
            ModelState.Clear();
            var viewData = await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(model).ConfigureAwait(false);
            return View(viewData.Item1, viewData.Item2);
        }

        [HttpPost]
        public async Task<ActionResult> JustToBeSafeNext(JustToBeSafeViewModel model)
        {
            ModelState.Clear();
            var next = await _justToBeSafeViewModelBuilder.JustToBeSafeNextBuilder(model).ConfigureAwait(false);
            return View(next.Item1, next.Item2);
        }

        [HttpGet]
        [Route("Covid-19/SMS")]
        public async Task<ActionResult> SmsFirstQuestion(JustToBeSafeViewModel model)
        {
            var firstModel = BuildModel("PC111", model);
            return await JustToBeSafeFirst(firstModel).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("{pathwayNumber}/{gender}/{age}/start")]
        public async Task<ActionResult> FirstQuestion(string pathwayNumber, string gender, int age, string args)
        {
            if (FeatureRouter.CovidSearchRedirect(HttpContext.Request.Params) && pathwayNumber == GuidedSelectionPathwayNumber)
            {
                return RedirectToAction("GuidedSelection", new RouteValueDictionary
                {
                    { "gender",gender},
                    { "age", age},
                    { "args", args}
                });
            }

            var model = BuildModel(pathwayNumber, gender, age, args);
            return await JustToBeSafeFirst(model).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("Question/First")]
        public async Task<ActionResult> FirstQuestionDeeplink(JustToBeSafeViewModel model)
        {
            ModelState.Clear();
            return await JustToBeSafeFirst(model).ConfigureAwait(false);
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
                FilterServices = bool.Parse(decryptedArgs["filterServices"]),
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                IsCovidJourney = bool.Parse(decryptedArgs["isCovidjourney"]),
                EntrySearchTerm = decryptedArgs["entrySearchTerm"],
                ViaGuidedSelection = true,
                StartParameter = decryptedArgs["startParameter"]
            };

            var ageGroup = new AgeCategory(model.UserInfo.Demography.Age);
            var requestPath = _configuration.GetBusinessApiGuidedPathwaySearchUrl(model.UserInfo.Demography.Gender, ageGroup.Value, true);

            var request = new RestRequest(requestPath, Method.POST);

            request.AddJsonBody(new { query = SearchReservedCovidTerms.SearchTerms.First() });

            var response = await _restClientBusinessApi.ExecuteAsync<List<GuidedSearchResultViewModel>>(request).ConfigureAwait(false);

            var guidedModel = Mapper.Map<GuidedSearchJourneyViewModel>(model);
            guidedModel.GuidedResults = response.Data;

            return !guidedModel.GuidedResults.Any() ? View("~\\Views\\Search\\NoResults.cshtml", model) : View("~\\Views\\Search\\GuidedSelection.cshtml", guidedModel);
        }

        private static QuestionInfoViewModel BuildModel(string pathwayNumber, JustToBeSafeViewModel jtbsModel)
        {
            var userInfo = jtbsModel.UserInfo;
            var demogs = userInfo != null ? jtbsModel.UserInfo.Demography : null;
            var age = userInfo != null &&
                      demogs != null && demogs.Age > 0
                ? demogs.Age
                : 111;

            var gender = userInfo != null &&
                      demogs != null && !string.IsNullOrEmpty(demogs.Gender)
                ? demogs.Gender
                : "Male";

            var state = new Dictionary<string, string>();
            state.Add("PATIENT_GENDER", string.Format("\"{0}\"", gender.Substring(0, 1)));
            state.Add("SYMPTOMS_STARTED_DAYS_AGO", "-1");
            if (demogs != null && demogs.Age > 0)
                state.Add("PATIENT_AGE", age.ToString());
            else
                state.Add("PATIENT_AGE", "-1");


            var model = new QuestionInfoViewModel
            {
                SessionId = jtbsModel.SessionId,
                PathwayNo = pathwayNumber,
                EntrySearchTerm = string.Format("External get to {0}", pathwayNumber),
                State = state,
                StateJson = JsonConvert.SerializeObject(state),
                UserInfo = new UserInfo
                {
                    Demography = new AgeGenderViewModel
                    {
                        Age = age,
                        Gender = gender
                    }
                },
                CurrentPostcode = jtbsModel.CurrentPostcode
            };
            return model;
        }

        private static QuestionInfoViewModel BuildModel(string pathwayNumber, string gender, int age, string args)
        {
            var decryptedArgs = new QueryStringEncryptor(args);
            var decryptedFilterServices = !decryptedArgs.ContainsKey("filterServices") || string.IsNullOrEmpty(decryptedArgs["filterServices"]) ||
                                          bool.Parse(decryptedArgs["filterServices"]);

            var model = new QuestionInfoViewModel
            {
                SessionId = Guid.Parse(decryptedArgs["sessionId"]),
                PathwayNo = pathwayNumber,
                DigitalTitle = decryptedArgs["digitalTitle"],
                EntrySearchTerm = decryptedArgs["entrySearchTerm"],
                CurrentPostcode = decryptedArgs["postcode"],
                UserInfo = new UserInfo
                {
                    Demography = new AgeGenderViewModel
                    {
                        Age = age,
                        Gender = gender
                    }
                },
                FilterServices = decryptedFilterServices,
                Campaign = decryptedArgs["campaign"],
                Source = decryptedArgs["source"],
                ViaGuidedSelection = string.IsNullOrEmpty(decryptedArgs["viaGuidedSelection"]) ? (bool?)null : bool.Parse(decryptedArgs["viaGuidedSelection"]),
                StartParameter = decryptedArgs["startParameter"]
            };
            return model;
        }
    }
}