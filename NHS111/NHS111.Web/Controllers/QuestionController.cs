using NHS111.Models.Models.Web.Validators;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Filters;

namespace NHS111.Web.Controllers
{
    using Features;
    using Helpers;
    using Models.Models.Domain;
    using Models.Models.Web;
    using Models.Models.Web.DosRequests;
    using Models.Models.Web.Enums;
    using Newtonsoft.Json;
    using Presentation.Builders;
    using Presentation.Configuration;
    using Presentation.Logging;
    using Presentation.ModelBinders;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Utils.Attributes;
    using IConfiguration = Presentation.Configuration.IConfiguration;

    [LogHandleErrorForMVC]
    public class QuestionController : Controller
    {

        public QuestionController(IJourneyViewModelBuilder journeyViewModelBuilder,
            IConfiguration configuration, IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder, IDirectLinkingFeature directLinkingFeature,
            IAuditLogger auditLogger, IUserZoomDataBuilder userZoomDataBuilder, ILoggingRestClient restClientBusinessApi, IViewRouter viewRouter,
            IDosEndpointFeature dosEndpointFeature, IDOSSpecifyDispoTimeFeature dosSpecifyDispoTimeFeature, IOutcomeViewModelBuilder outcomeViewModelBuilder)
        {

            _journeyViewModelBuilder = journeyViewModelBuilder;
            _configuration = configuration;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _directLinkingFeature = directLinkingFeature;
            _auditLogger = auditLogger;
            _userZoomDataBuilder = userZoomDataBuilder;
            _restClientBusinessApi = restClientBusinessApi;
            _viewRouter = viewRouter;
            _dosEndpointFeature = dosEndpointFeature;
            _dosSpecifyDispoTimeFeature = dosSpecifyDispoTimeFeature;
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _questionNavigiationService = new QuestionNavigationService(_journeyViewModelBuilder, _configuration,
                _restClientBusinessApi, _viewRouter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModuleZero(JourneyViewModel model)
        {
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
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AutosuggestPathways(string input, string gender, int age)
        {

            var response = await _restClientBusinessApi.ExecuteAsync<List<GroupedPathways>>(
                     new RestRequest(_configuration.GetBusinessApiGroupedPathwaysUrl(input, gender, age, true), Method.GET))
                .ConfigureAwait(false);

            return Json(await Search(response.Data).ConfigureAwait(false));
        }

        private async Task<string> Search(List<GroupedPathways> pathways)
        {

            return
                JsonConvert.SerializeObject(
                    pathways.Select(pathway => new { label = pathway.Group, value = pathway.PathwayNumbers }));
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "Question")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Question(QuestionViewModel model)
        {
            if (!ModelState.IsValidField("SelectedAnswer") ||
                !ModelState.IsValidField("AnswerInputValue") ||
                !ModelState.IsValidField("DateAnswer"))
            {
                return View(_viewRouter.Build(model, ControllerContext).ViewName, model);
            }


            if (model.NodeType == NodeType.Page && model.Content != null && model.Content.StartsWith("!CustomView!"))
                return await HandleCustomQuestion(model).ConfigureAwait(false); //Refactor into custom Handler Class
            ModelState.Clear();

            var nextModel = await _questionNavigiationService.GetNextJourneyViewModel(model).ConfigureAwait(false);
            var viewRouter = _viewRouter.Build(nextModel, ControllerContext);

            return View(viewRouter.ViewName, nextModel);
        }

        private async Task<ActionResult> HandleCustomQuestion(QuestionViewModel model)
        {
            var days = Request.Params["SymptomsStart.Day"];
            int daysInt;
            if (!int.TryParse(days, out daysInt))
            {
                ModelState.AddModelError("SymptomsStart.Day", "Please enter a number");
                return View("Custom/SymptomsStarted", model);
            }
            var result = new QuestionViewModelValidator.IntegerSymptomsStartedValidator().Validate(days);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("SymptomsStart.Day", error.ErrorMessage);
                }

                return View("Custom/SymptomsStarted", model);
            }

            _auditLogger.LogEvent(model, EventType.SymptomsBeganDate, DateTime.Now.AddDays(daysInt * -1).Date.ToString("s"), "../Question/Custom/SymptomsStarted");
            ModelState.Clear();

            var state = JsonConvert.DeserializeObject<IDictionary<string, string>>(model.StateJson);
            if (!state.ContainsKey("SYMPTOMS_STARTED_DAYS_AGO"))
                state.Add("SYMPTOMS_STARTED_DAYS_AGO", days);
            else
                state["SYMPTOMS_STARTED_DAYS_AGO"] = days;
            model.StateJson = JsonConvert.SerializeObject(state);
            var nextModel = await _questionNavigiationService.GetNextJourneyViewModel(model).ConfigureAwait(false);
            var viewRouter = _viewRouter.Build(nextModel, ControllerContext);

            return View(viewRouter.ViewName, nextModel);

        }

        private JourneyViewModel GetMatchingTestJourney(OutcomeViewModel model)
        {
            var testJourneys = ReadTestJourneys();

            var comparer = new JourneyViewModelEqualityComparer();
            foreach (var testJourney in testJourneys)
            {
                var result = JsonConvert.DeserializeObject<JourneyViewModel>(testJourney.Json);
                if (comparer.Equals(model, result))
                {
                    model.TriggerQuestionNo = testJourney.TriggerQuestionNo;
                    return result;
                }
            }

            return null;
        }

        private static IEnumerable<TestJourneyElement> ReadTestJourneys()
        {
            var section = ConfigurationManager.GetSection("testJourneySection");
            if (!(section is TestJourneysConfigurationSection))
                return new List<TestJourneyElement>();

            return (section as TestJourneysConfigurationSection)
                .TestJourneys
                .Cast<TestJourneyElement>();
        }


        [HttpPost]
        [ActionName("NextNodeDetails")]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetNextNodeDetails(QuestionViewModel model)
        {
            var nodeDetails = new NodeDetailsViewModel() { NodeType = NodeType.Question };
            if (ModelState.IsValidField("SelectedAnswer"))
            {
                var nextNode = await _questionNavigiationService.GetNextNode(model).ConfigureAwait(false);
                nodeDetails = _journeyViewModelBuilder.BuildNodeDetails(nextNode);
            }

            return Json(nodeDetails);
        }


        [HttpGet]
        public ActionResult InitialQuestion()
        {
            var model = new JourneyViewModel();
            var audit = model.ToAuditEntry();
            audit.EventData = "User directed from duplicate submission page";
            _auditLogger.Log(audit);

            model.UserInfo = new UserInfo();
            _userZoomDataBuilder.SetFieldsForDemographics(model);
            return View("Gender", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InitialQuestion(JourneyViewModel model)
        {
            var audit = model.ToAuditEntry();
            audit.EventData = "User accepted module zero.";
            _auditLogger.Log(audit);

            ModelState.Clear();
            model.UserInfo = new UserInfo() { CurrentAddress = new FindServicesAddressViewModel() { Postcode = model.UserInfo.CurrentAddress.Postcode } };

            _userZoomDataBuilder.SetFieldsForDemographics(model);
            return View("Gender", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> PathwaySearch(string gender, int age, string searchTerm)
        {
            var ageGroup = new AgeCategory(age);
            var response =
                await
                    _restClientBusinessApi.ExecuteAsync(
                        new RestRequest(_configuration.GetBusinessApiPathwaySearchUrl(gender, ageGroup.Value, true),
                            Method.GET))
                        .ConfigureAwait(false);

            return Json(response.Content);
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "CheckAnswer")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Revisit(OutcomeViewModel model,
            [ModelBinder(typeof(IntArrayModelBinder))] int[] answers,
            bool? filterServices, string selectedAnswer)
        {

            if (selectedAnswer.ToLower() == "no")
            {
                model = await _outcomeViewModelBuilder.DispositionBuilder(model).ConfigureAwait(false);
                if (model.DosCheckCapacitySummaryResult.HasITKServices)
                {
                    throw new NotImplementedException(); //no trigger question journeys currently offer callback
                }
                var viewRouter = _viewRouter.Build(model, ControllerContext);
                return View(viewRouter.ViewName, model);
            }

            var result = await DirectInternal(model.PathwayId, model.UserInfo.Demography.Age, model.PathwayTitle, model.CurrentPostcode, answers, filterServices)
                .ConfigureAwait(false);

            var journeyViewModel = (JourneyViewModel)((ViewResult)result).Model;
            journeyViewModel.TriggerQuestionNo = model.TriggerQuestionNo;
            journeyViewModel.TriggerQuestionAnswer = model.TriggerQuestionAnswer;
            journeyViewModel.JourneyId = model.JourneyId;
            journeyViewModel.SessionId = model.SessionId;
            ModelState.Clear();
            return result;
        }

        [HttpGet]
        [Route("question/direct/{pathwayId}/{age?}/{pathwayTitle}/{postcode}/{answers?}")]
        public async Task<ActionResult> Direct(string pathwayId, int? age, string pathwayTitle, string postcode, [ModelBinder(typeof(IntArrayModelBinder))]int[] answers, bool? filterServices)
        {

            if (!_directLinkingFeature.IsEnabled)
            {
                return HttpNotFound();
            }

            return await DirectInternal(pathwayId, age, pathwayTitle, postcode, answers, filterServices)
                .ConfigureAwait(false);
        }

        public async Task<ActionResult> DirectInternal(string pathwayId, int? age, string pathwayTitle, string postcode, [ModelBinder(typeof(IntArrayModelBinder))] int[] answers, bool? filterServices)
        {
            var resultingModel = await DeriveJourneyView(pathwayId, age, pathwayTitle, answers)
                .ConfigureAwait(false);
            resultingModel.CurrentPostcode = postcode;
            resultingModel.TriggerQuestionNo = null;
            if (resultingModel != null)
            {
                resultingModel.FilterServices = filterServices.HasValue ? filterServices.Value : true;

                if (resultingModel.NodeType == NodeType.Outcome)
                {
                    var outcomeModel = resultingModel as OutcomeViewModel;

                    DosEndpoint? endpoint = SetEndpoint();
                    DateTime? dosSearchTime = null;
                    if (_dosSpecifyDispoTimeFeature.IsEnabled && _dosSpecifyDispoTimeFeature.HasDate(Request))
                        dosSearchTime = _dosSpecifyDispoTimeFeature.GetDosSearchDateTime(Request);

                    outcomeModel.CurrentView = _viewRouter.Build(resultingModel, ControllerContext).ViewName;

                    var controller = DependencyResolver.Current.GetService<OutcomeController>();
                    controller.ControllerContext = new ControllerContext(ControllerContext.RequestContext, controller);

                    if (OutcomeGroup.PrePopulatedDosResultsOutcomeGroups.Contains(outcomeModel.OutcomeGroup))
                        return await DeterminePrepopulatedResultsRoute(controller, outcomeModel, endpoint, dosSearchTime)
                            .ConfigureAwait(false);

                    if (OutcomeGroup.DosSearchOutcomesGroups.Contains(outcomeModel.OutcomeGroup))
                        return await controller.ServiceList(outcomeModel, dosSearchTime, null, endpoint)
                            .ConfigureAwait(false);
                }
            }

            var viewRouter = _viewRouter.Build(resultingModel, ControllerContext);
            return View(viewRouter.ViewName, resultingModel);
        }

        private DosEndpoint? SetEndpoint()
        {
            if (!_dosEndpointFeature.IsEnabled)
                return null;

            switch (_dosEndpointFeature.GetEndpoint(Request))
            {
                case "uat":
                    return DosEndpoint.UAT;
                case "live":
                    return DosEndpoint.Live;
                default:
                    return null;
            }
        }

        private bool DirectToOtherServices
        {
            get
            {
                switch (Request.QueryString["otherservices"])
                {
                    case "true":
                        return true;
                    default:
                        return false;
                }
            }
        }

        private async Task<ActionResult> DeterminePrepopulatedResultsRoute(OutcomeController controller, OutcomeViewModel outcomeViewModel, DosEndpoint? endpoint = null, DateTime? dosSearchTime = null)
        {
            var dispoWithServicesResult = await controller.DispositionWithServices(outcomeViewModel, "", endpoint, dosSearchTime)
                .ConfigureAwait(false);

            if (!OutcomeGroup.UsingRecommendedServiceJourney.Contains(outcomeViewModel.OutcomeGroup) && !outcomeViewModel.OutcomeGroup.IsPrimaryCare)
                return dispoWithServicesResult;

            var dispoWithServicesView = dispoWithServicesResult as ViewResult;
            if (dispoWithServicesView.ViewName != "../Outcome/Repeat_Prescription/Outcome_Preamble" && !outcomeViewModel.OutcomeGroup.IsPrimaryCare)
                return View(dispoWithServicesView.ViewName, dispoWithServicesView.Model);

            // need to do the first look up to determine if there are other services
            var outcomeModel = dispoWithServicesView.Model as OutcomeViewModel;
            if (!DirectToOtherServices)
                return controller.RecommendedService(outcomeModel);

            if (!outcomeModel.OutcomeGroup.IsUsingRecommendedService)
                outcomeModel.RecommendedService = null;

            var minimumServicesNeededForServiceList = outcomeModel.OutcomeGroup.IsUsingRecommendedService ? 2 : 1;

            if (outcomeModel.DosCheckCapacitySummaryResult.Success.Services.Count >= minimumServicesNeededForServiceList)
                return await controller.ServiceList(outcomeModel, dosSearchTime, null, endpoint)
                    .ConfigureAwait(false);

            return View("../Outcome/Repeat_Prescription/RecommendedServiceNotOffered", outcomeModel);
        }

        private async Task<JourneyViewModel> DeriveJourneyView(string pathwayId, int? age, string pathwayTitle, int[] answers)
        {
            var questionViewModel = BuildQuestionViewModel(pathwayId, age, pathwayTitle);
            var response = await
                _restClientBusinessApi.ExecuteAsync<Pathway>(new JsonRestRequest(_configuration.GetBusinessApiPathwayUrl(pathwayId, true), Method.GET))
                    .ConfigureAwait(false);
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
            questionViewModel = (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(newModel).ConfigureAwait(false)).Item2; //todo refactor tuple away

            var resultingModel = await AnswerQuestions(questionViewModel, answers)
                .ConfigureAwait(false);
            return resultingModel;
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "PreviousQuestion")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PreviousQuestion(QuestionViewModel model)
        {
            ModelState.Clear();

            var url = _configuration.GetBusinessApiQuestionByIdUrl(model.PathwayId, model.Journey.Steps.Last().QuestionId, true);
            var response = await _restClientBusinessApi.ExecuteAsync<QuestionWithAnswers>(new JsonRestRequest(url, Method.GET))
                .ConfigureAwait(false);
            var questionWithAnswers = response.Data;

            var result = _journeyViewModelBuilder.BuildPreviousQuestion(questionWithAnswers, model);
            var viewRouter = _viewRouter.Build(result, ControllerContext);

            return View(viewRouter.ViewName, result);
        }

        private async Task<List<QuestionWithAnswers>> GetFullJourney(QuestionViewModel model)
        {
            var request = new JsonRestRequest(_configuration.BusinessApiGetFullPathwayJourneyUrl, Method.POST);
            request.AddJsonBody(model.Journey.Steps.ToArray());
            var response = await _restClientBusinessApi.ExecuteAsync<List<QuestionWithAnswers>>(request)
                .ConfigureAwait(false);
            return response.Data;
        }

        private async Task<JourneyViewModel> AnswerQuestions(QuestionViewModel model, int[] answers)
        {
            if (answers == null)
                return model;

            var queue = new Queue<int>(answers);
            var journeyViewModel = new JourneyViewModel();
            while (queue.Any())
            {
                var answer = queue.Dequeue();
                journeyViewModel = await AnswerQuestion(model, answer)
                    .ConfigureAwait(false);
            }
            return journeyViewModel;
        }

        private async Task<JourneyViewModel> AnswerQuestion(QuestionViewModel model, int answer)
        {
            if (answer < 0 || answer >= model.Answers.Count)
                throw new ArgumentOutOfRangeException(
                    string.Format("The answer index '{0}' was not found within the range of answers: {1}", answer,
                        string.Join(", ", model.Answers.Select(a => a.Title))));

            model.SelectedAnswer = JsonConvert.SerializeObject(model.Answers.First(a => a.Order == answer + 1));
            var result = (ViewResult)await Question(model).ConfigureAwait(false);

            return result.Model is OutcomeViewModel ? (OutcomeViewModel)result.Model : (JourneyViewModel)result.Model;
        }

        private static QuestionViewModel BuildQuestionViewModel(string pathwayId, int? age, string pathwayTitle)
        {
            return new QuestionViewModel
            {
                NodeType = NodeType.Pathway,
                PathwayId = pathwayId,
                PathwayTitle = pathwayTitle,
                UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Age = age ?? -1 } }
            };
        }

        private readonly IJourneyViewModelBuilder _journeyViewModelBuilder;
        private readonly IConfiguration _configuration;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
        private readonly IDirectLinkingFeature _directLinkingFeature;
        private readonly IAuditLogger _auditLogger;
        private readonly IUserZoomDataBuilder _userZoomDataBuilder;
        private readonly ILoggingRestClient _restClientBusinessApi;
        private readonly IViewRouter _viewRouter;
        private readonly IDosEndpointFeature _dosEndpointFeature;
        private readonly IDOSSpecifyDispoTimeFeature _dosSpecifyDispoTimeFeature;
        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IQuestionNavigiationService _questionNavigiationService;
    }
}
