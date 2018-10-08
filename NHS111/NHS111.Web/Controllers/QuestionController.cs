
namespace NHS111.Web.Controllers {
    using Features;
    using Helpers;
    using RestSharp;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models.Models.Web;
    using Models.Models.Web.Enums;
    using Utils.Attributes;
    using Presentation.Builders;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using Models.Models.Domain;
    using Models.Models.Web.DosRequests;
    using Newtonsoft.Json;
    using Presentation.Configuration;
    using Presentation.Logging;
    using Presentation.ModelBinders;
    using Utils.Filters;
    using IConfiguration = Presentation.Configuration.IConfiguration;

    [LogHandleErrorForMVC]
    public class QuestionController : Controller {

        public QuestionController(IJourneyViewModelBuilder journeyViewModelBuilder,
            IConfiguration configuration, IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder, IDirectLinkingFeature directLinkingFeature,
            IAuditLogger auditLogger, IUserZoomDataBuilder userZoomDataBuilder, IRestClient restClientBusinessApi, IViewRouter viewRouter, IPostcodePrefillFeature postcodePrefillFeature, IDosEndpointFeature dosEndpointFeature, IDOSSpecifyDispoTimeFeature dosSpecifyDispoTimeFeature,
            ISurveyLinkViewModelBuilder surveyBuilder) {
            _journeyViewModelBuilder = journeyViewModelBuilder;
            _configuration = configuration;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _directLinkingFeature = directLinkingFeature;
            _auditLogger = auditLogger;
            _userZoomDataBuilder = userZoomDataBuilder;
            _restClientBusinessApi = restClientBusinessApi;
            _viewRouter = viewRouter;
            _postcodePrefillFeature = postcodePrefillFeature;
            _dosEndpointFeature = dosEndpointFeature;
            _dosSpecifyDispoTimeFeature = dosSpecifyDispoTimeFeature;
            _surveyBuilder = surveyBuilder;
        }

        [HttpGet, PersistCampaignDataFilter]
        public ActionResult Home(JourneyViewModel model, string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                var decryptedFields = new QueryStringEncryptor(args);
                model.CurrentPostcode = decryptedFields["postcode"];
                model.SessionId = Guid.Parse(decryptedFields["sessionId"]);
            }

            _userZoomDataBuilder.SetFieldsForInitialQuestion(model);
            return View("InitialQuestion", model);
        }

        [HttpGet]
        [Route("seleniumtests/direct/{postcode}")]
        public ActionResult SeleniumTesting(string postcode, bool filterServices = true)
        {
            var startOfJourney = new JourneyViewModel
            {
                SessionId = Guid.Parse(Request.AnonymousID),
                FilterServices = filterServices,
                UserInfo = new UserInfo
                {
                    CurrentAddress = new FindServicesAddressViewModel()
                    {
                        Postcode = postcode
                    }
                }
            };

            _userZoomDataBuilder.SetFieldsForHome(startOfJourney);
            return View("InitialQuestion", startOfJourney);
        }

        [HttpPost]
        public async Task<JsonResult> AutosuggestPathways(string input, string gender, int age)
        {
 
            var response = await _restClientBusinessApi.ExecuteTaskAsync<List<GroupedPathways>>(
                     new RestRequest(_configuration.GetBusinessApiGroupedPathwaysUrl(input, gender, age, true), Method.GET));

            return Json(await Search(response.Data));
        }

        private async Task<string> Search(List<GroupedPathways> pathways)
        {
            
            return
                JsonConvert.SerializeObject(
                    pathways.Select(pathway => new {label = pathway.Group, value = pathway.PathwayNumbers}));
        }
        
        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "Question")]
        public async Task<ActionResult> Question(QuestionViewModel model)
        {
            if (!ModelState.IsValidField("SelectedAnswer")) return View("Question", model);

            ModelState.Clear();
            var nextModel = await GetNextJourneyViewModel(model);

            var viewName = _viewRouter.GetViewName(nextModel, ControllerContext);
            return View(viewName, nextModel);

        }

        private JourneyViewModel GetMatchingTestJourney(OutcomeViewModel model) {
            var testJourneys = ReadTestJourneys();

            //var json = "{\"PathwayNo\":\"PW755\", \"UserInfo\":{ \"Demography\":{ \"Gender\":\"Male\",\"Age\":22}},\"Journey\":{\"Steps\":[{\"questionId\":\"Tx123\",\"answer\":{ \"title\":\"Yes\" }}, {\"questionId\":\"Tx123\",\"answer\":{ \"title\":\"Yes\" }}]}}";
            var comparer = new JourneyViewModelEqualityComparer();
            foreach (var testJourney in testJourneys) {
                var result = JsonConvert.DeserializeObject<JourneyViewModel>(testJourney.Json);
                if (comparer.Equals(model, result)) {
                    model.TriggerQuestionNo = testJourney.TriggerQuestionNo;
                    return result;
                }
            }

            return null;
        }

        private static IEnumerable<TestJourneyElement> ReadTestJourneys() {
            var section = ConfigurationManager.GetSection("testJourneySection");
            if (!(section is TestJourneysConfigurationSection))
                return new List<TestJourneyElement>();

            return (section as TestJourneysConfigurationSection)
                .TestJourneys
                .Cast<TestJourneyElement>();
        }


        [HttpPost]
        [ActionName("NextNodeDetails")]
        public async Task<JsonResult> GetNextNodeDetails(QuestionViewModel model)
        {
            var nodeDetails = new NodeDetailsViewModel() { NodeType = NodeType.Question };
            if (ModelState.IsValidField("SelectedAnswer"))
            {
                var nextNode = await GetNextNode(model);
                nodeDetails = _journeyViewModelBuilder.BuildNodeDetails(nextNode);
            }
            
            return Json(nodeDetails);
        }


        [HttpGet]
        public async Task<ActionResult> InitialQuestion()
        {
            var model = new JourneyViewModel();
            var audit = model.ToAuditEntry();
            audit.EventData = "User directed from duplicate submission page";
            await _auditLogger.Log(audit);

            model.UserInfo = new UserInfo();
            _userZoomDataBuilder.SetFieldsForDemographics(model);
            return View("Gender", model);
        }

        [HttpPost]
        public async Task<ActionResult> InitialQuestion(JourneyViewModel model)
        {
            var audit = model.ToAuditEntry();
            audit.EventData = "User accepted module zero.";
            await _auditLogger.Log(audit);

            ModelState.Clear();
            model.UserInfo = new UserInfo() { CurrentAddress = new FindServicesAddressViewModel() { Postcode = model.UserInfo.CurrentAddress.Postcode } };
            
            _userZoomDataBuilder.SetFieldsForDemographics(model);
            return View("Gender", model);
        }

        [HttpPost]
        public async Task<JsonResult> PathwaySearch(string gender, int age, string searchTerm) {
            var ageGroup = new AgeCategory(age);
            var response =
                await
                    _restClientBusinessApi.ExecuteTaskAsync(
                        new RestRequest(_configuration.GetBusinessApiPathwaySearchUrl(gender, ageGroup.Value, true),
                            Method.GET));

            return Json(response.Content);
        }


        private async Task<JourneyViewModel> GetNextJourneyViewModel(QuestionViewModel model) {
            var nextNode = await GetNextNode(model);
            return await _journeyViewModelBuilder.Build(model, nextNode);
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "CheckAnswer")]
        //[Route("question/revisit/{questionNo}/")]
        public async Task<ActionResult> Revisit(OutcomeViewModel model, 
            [ModelBinder(typeof(IntArrayModelBinder))] int[] answers,
            bool? filterServices, string selectedAnswer) {

            if (selectedAnswer.ToLower() == "no") {
                var viewName = _viewRouter.GetViewName(model, ControllerContext);
                model.SurveyLink = await _surveyBuilder.SurveyLinkBuilder(model);
                return View(viewName, model);
            }

            var result = await DirectInternal(model.PathwayId, null, model.PathwayTitle, answers, filterServices);

            var journeyViewModel = (JourneyViewModel) ((ViewResult) result).Model;
            journeyViewModel.TriggerQuestionNo = model.TriggerQuestionNo;
            journeyViewModel.JourneyId = model.JourneyId;
            journeyViewModel.SessionId = model.SessionId;
            ModelState.Clear();
            return result;
        }

        [HttpGet]
        [Route("question/direct/{pathwayId}/{age?}/{pathwayTitle}/{answers?}")]
        public async Task<ActionResult> Direct(string pathwayId, int? age, string pathwayTitle,
            [ModelBinder(typeof(IntArrayModelBinder))]
            int[] answers, bool? filterServices) {

            if (!_directLinkingFeature.IsEnabled) {
                return HttpNotFound();
            }

            return await DirectInternal(pathwayId, age, pathwayTitle, answers, filterServices);
        }

        public async Task<ActionResult> DirectInternal(string pathwayId, int? age, string pathwayTitle,
            [ModelBinder(typeof(IntArrayModelBinder))] int[] answers, bool? filterServices) {
            var resultingModel = await DeriveJourneyView(pathwayId, age, pathwayTitle, answers);
            if (resultingModel != null) {
                resultingModel.FilterServices = filterServices.HasValue ? filterServices.Value : true;

                if (resultingModel.NodeType == NodeType.Outcome) {
                    var outcomeModel = resultingModel as OutcomeViewModel;

                    bool shouldPrefillPostcode = _postcodePrefillFeature.IsEnabled &&
                                                 OutcomeGroup.DosSearchOutcomesGroups.Contains(outcomeModel.OutcomeGroup) &&
                                                 _postcodePrefillFeature.RequestIncludesPostcode(Request);

                    DosEndpoint? endpoint = SetEndpoint();
                    DateTime? dosSearchTime = null;
                    if (_dosSpecifyDispoTimeFeature.IsEnabled && _dosSpecifyDispoTimeFeature.HasDate(Request))
                        dosSearchTime = _dosSpecifyDispoTimeFeature.GetDosSearchDateTime(Request);

                    if (shouldPrefillPostcode) {
                        resultingModel.CurrentPostcode = _postcodePrefillFeature.GetPostcode(Request);
                        outcomeModel.CurrentView = _viewRouter.GetViewName(resultingModel, ControllerContext);

                        var controller = DependencyResolver.Current.GetService<OutcomeController>();
                            controller.ControllerContext = new ControllerContext(ControllerContext.RequestContext,
                                controller);
                        if (OutcomeGroup.PrePopulatedDosResultsOutcomeGroups.Contains(outcomeModel.OutcomeGroup))
                            return await controller.DispositionWithServices(outcomeModel, "", endpoint, dosSearchTime);
                
                        return await controller.ServiceList(outcomeModel, dosSearchTime, null, endpoint);
                        
                    }
                }
            }

            var viewName = _viewRouter.GetViewName(resultingModel, ControllerContext);
            return View(viewName, resultingModel);
        }

        private DosEndpoint? SetEndpoint() {
            if (!_dosEndpointFeature.IsEnabled)
                return null;

            switch (_dosEndpointFeature.GetEndpoint(Request)) {
                case "uat":
                    return DosEndpoint.UAT;
                case "live":
                    return DosEndpoint.Live;
                default:
                    return null;
            }
        }

        [HttpGet]
        [Route("question/outcomedetail/{pathwayId}/{age?}/{pathwayTitle}/{answers?}")]
        public async Task<ActionResult> OutcomeDetail(string pathwayId, int? age, string pathwayTitle,
            [ModelBinder(typeof(IntArrayModelBinder))] int[] answers, bool? filterServices)
        {

            var journeyViewModel = await DeriveJourneyView(pathwayId, age, pathwayTitle, answers);
            if(journeyViewModel != null)
                journeyViewModel.FilterServices = filterServices.HasValue ? filterServices.Value : true;

            var viewName = _viewRouter.GetViewName(journeyViewModel, ControllerContext);
            if (journeyViewModel.OutcomeGroup == null ||
                !OutcomeGroup.SignpostingOutcomesGroups.Contains(journeyViewModel.OutcomeGroup))
            {
                return HttpNotFound();
            }
            journeyViewModel.DisplayOutcomeReferenceOnly = true;
            return View(viewName, journeyViewModel);
        }

        private async Task<JourneyViewModel> DeriveJourneyView(string pathwayId, int? age, string pathwayTitle, int[] answers)
        {
            var questionViewModel = BuildQuestionViewModel(pathwayId, age, pathwayTitle);
            var response = await 
                _restClientBusinessApi.ExecuteTaskAsync<Pathway>(
                    CreateJsonRequest(_configuration.GetBusinessApiPathwayUrl(pathwayId, true), Method.GET));
            var pathway = response.Data;
            if (pathway == null) return null;

            var derivedAge = questionViewModel.UserInfo.Demography.Age == -1 ? pathway.MinimumAgeInclusive : questionViewModel.UserInfo.Demography.Age;
            var newModel = new JustToBeSafeViewModel
            {
                PathwayId = pathway.Id,
                PathwayNo = pathway.PathwayNo,
                PathwayTitle = pathway.Title,
                DigitalTitle = string.IsNullOrEmpty(questionViewModel.DigitalTitle) ? pathway.Title : questionViewModel.DigitalTitle,
                UserInfo = new UserInfo() { Demography = new AgeGenderViewModel { Age = derivedAge, Gender = pathway.Gender } },
                JourneyJson = questionViewModel.JourneyJson,
                SymptomDiscriminatorCode = questionViewModel.SymptomDiscriminatorCode,
                State = JourneyViewModelStateBuilder.BuildState(pathway.Gender, derivedAge),
            };

            newModel.StateJson = JourneyViewModelStateBuilder.BuildStateJson(newModel.State);
            questionViewModel = (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(newModel)).Item2; //todo refactor tuple away

            var resultingModel = await AnswerQuestions(questionViewModel, answers);
            return resultingModel;
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "PreviousQuestion")]
        public async Task<ActionResult> PreviousQuestion(QuestionViewModel model) {
            ModelState.Clear();

            var url = _configuration.GetBusinessApiQuestionByIdUrl(model.PathwayId, model.Journey.Steps.Last().QuestionId, true);
            var response = await _restClientBusinessApi.ExecuteTaskAsync<QuestionWithAnswers>(CreateJsonRequest(url, Method.GET));
            var questionWithAnswers = response.Data;

            var result = _journeyViewModelBuilder.BuildPreviousQuestion(questionWithAnswers, model);
            var viewName = _viewRouter.GetViewName(result, ControllerContext);

            return View(viewName, result);
        }

        private async Task<QuestionWithAnswers> GetNextNode(QuestionViewModel model) {
            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);
            var serialisedState = HttpUtility.UrlEncode(model.StateJson);
            var request = CreateJsonRequest(_configuration.GetBusinessApiNextNodeUrl(model.PathwayId, model.NodeType, model.Id, serialisedState, true), Method.POST);
            request.AddJsonBody(answer.Title);
            var response = await _restClientBusinessApi.ExecuteTaskAsync<QuestionWithAnswers>(request);
            return response.Data;
        }

        private async Task<List<QuestionWithAnswers>> GetFullJourney(QuestionViewModel model)
        {
            var request = CreateJsonRequest(_configuration.BusinessApiGetFullPathwayJourneyUrl, Method.POST);
            request.AddJsonBody(model.Journey.Steps.ToArray());
            var response = await _restClientBusinessApi.ExecuteTaskAsync<List<QuestionWithAnswers>>(request);
            return response.Data;
        }

        private async Task<JourneyViewModel> AnswerQuestions(QuestionViewModel model, int[] answers) {
            if (answers == null)
                return model;

            var queue = new Queue<int>(answers);
            var journeyViewModel = new JourneyViewModel();
            while (queue.Any()) {
                var answer = queue.Dequeue();
                journeyViewModel = await AnswerQuestion(model, answer);
            }
            return journeyViewModel;
        }

        private async Task<JourneyViewModel> AnswerQuestion(QuestionViewModel model, int answer) {
            if (answer < 0 || answer >= model.Answers.Count)
                throw new ArgumentOutOfRangeException(
                    string.Format("The answer index '{0}' was not found within the range of answers: {1}", answer,
                        string.Join(", ", model.Answers.Select(a => a.Title))));

            model.SelectedAnswer = JsonConvert.SerializeObject(model.Answers.First(a => a.Order == answer + 1));
            var result = (ViewResult) await Question(model);

            return result.Model is OutcomeViewModel ? (OutcomeViewModel)result.Model : (JourneyViewModel)result.Model;
        }

        private static QuestionViewModel BuildQuestionViewModel(string pathwayId, int? age, string pathwayTitle) {
            return new QuestionViewModel
            {
                NodeType = NodeType.Pathway,
                PathwayId = pathwayId,
                PathwayTitle = pathwayTitle,
                UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Age = age ?? -1 }}
            };
        }

        private RestRequest CreateJsonRequest(string url, Method method)
        {
            var request =  new RestRequest(url, method);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            return request;
        }

        private readonly IJourneyViewModelBuilder _journeyViewModelBuilder;
        private readonly IConfiguration _configuration;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
        private readonly IDirectLinkingFeature _directLinkingFeature;
        private readonly IAuditLogger _auditLogger;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;
        private readonly IRestClient _restClientBusinessApi;
        private readonly IViewRouter _viewRouter;
        private readonly IPostcodePrefillFeature _postcodePrefillFeature;
        private readonly IDosEndpointFeature _dosEndpointFeature;
        private readonly IDOSSpecifyDispoTimeFeature _dosSpecifyDispoTimeFeature;
        private readonly ISurveyLinkViewModelBuilder _surveyBuilder;
    }
}
