
using AutoMapper;
using NHS111.Features;

namespace NHS111.Web.Controllers {
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models.Models.Web;
    using Models.Models.Web.Enums;
    using Utils.Attributes;
    using Utils.Parser;
    using Presentation.Builders;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Web;
    using Models.Models.Domain;
    using Newtonsoft.Json;
    using Presentation.Logging;
    using Presentation.ModelBinders;
    using Utils.Filters;
    using Utils.Helpers;
    using IConfiguration = Presentation.Configuration.IConfiguration;

    [LogHandleErrorForMVC]
    public class QuestionController
        : Controller {

        public QuestionController(IJourneyViewModelBuilder journeyViewModelBuilder, IRestfulHelper restfulHelper,
            IConfiguration configuration, IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder, IDirectLinkingFeature directLinkingFeature,
            IAuditLogger auditLogger) {
            _journeyViewModelBuilder = journeyViewModelBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _directLinkingFeature = directLinkingFeature;
            _auditLogger = auditLogger;
            }

        [HttpPost]
        public  ActionResult Home(JourneyViewModel model) {
            
            return View("InitialQuestion");
        }

        [HttpPost]
        public async Task<ActionResult> Search(AgeGenderViewModel model)
        {
            if (!ModelState.IsValid) return View("Gender", new JourneyViewModel { UserInfo = new UserInfo { Demography = model } });

            var response = await _restfulHelper.GetAsync(_configuration.GetBusinessApiGetCategoriesWithPathways());
            var allTopics = JsonConvert.DeserializeObject<List<CategoryWithPathways>>(response);
            var topicsContainingStartingPathways = allTopics.Where(c => c.Pathways.Any(p => p.Pathway.StartingPathway));

            var startOfJourney = new JourneyViewModel
            {
                UserInfo = new UserInfo { Demography = model },
                AllTopics = topicsContainingStartingPathways
            };

            return View(startOfJourney);
        }
        
        [HttpPost]
        public async Task<JsonResult> AutosuggestPathways(string input) {
            return Json(await Search(input));
        }

        private async Task<string> Search(string input) {
            var response = await _restfulHelper.GetAsync(_configuration.GetBusinessApiGroupedPathwaysUrl(input));
            var pathways = JsonConvert.DeserializeObject<List<GroupedPathways>>(response);
            return
                JsonConvert.SerializeObject(
                    pathways.Select(pathway => new {label = pathway.Group, value = pathway.PathwayNumbers}));
        }

        [HttpPost]
        public ActionResult Gender(JourneyViewModel model) {
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GenderDirect(string pathwayTitle) {
            var pathwayNo =
                await
                    _restfulHelper.GetAsync(
                        _configuration.GetBusinessApiPathwayNumbersUrl(PathwayTitleUriParser.Parse(pathwayTitle)));
            var model = new JourneyViewModel() { PathwayNo = pathwayNo, UserInfo = new UserInfo { Demography = new AgeGenderViewModel() } };

            if (model.PathwayNo == string.Empty)
                return Redirect(Url.RouteUrl(new {controller = "Question", action = "Home"}));
            return View("Gender", model);
        }

        [HttpGet]
        public ActionResult Home()
        {
            var startOfJourney = new JourneyViewModel
            {
                SessionId = Guid.Parse(Request.AnonymousID)
            };
            return View("Home", startOfJourney);
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "Question")]
        public async Task<ActionResult> Question(JourneyViewModel model) {
            ModelState.Clear();
            var nextModel = await GetNextJourneyViewModel(model);

            var viewName = DetermineViewName(nextModel);

            return View(viewName, nextModel);
        }

        
        [HttpPost]
        public async Task<ActionResult> InitialQuestion(JourneyViewModel model)
        {
            var audit = model.ToAuditEntry();
            audit.EventData = "User accepted module zero.";
            await _auditLogger.Log(audit);

            ModelState.Clear();
            model.UserInfo = new UserInfo();
            return View("Gender", model);
        }
        
        private async Task<JourneyViewModel> GetNextJourneyViewModel(JourneyViewModel model) {
            var nextNode = await GetNextNode(model);
            return await _journeyViewModelBuilder.Build(model, nextNode);
        }

        private string DetermineViewName(JourneyViewModel model) {

            if (model == null) return "../Question/Question";

            switch (model.NodeType) {
                case NodeType.Outcome:
                
                    var viewFilePath = "../Outcome/" + model.OutcomeGroup.Id;
                    if (ViewExists(viewFilePath))
                        return viewFilePath;
                    throw new ArgumentOutOfRangeException(string.Format("Outcome group {0} for outcome {1} has no view configured", model.OutcomeGroup.ToString(), model.Id));
                case NodeType.DeadEndJump:
                    return "../Question/DeadEndJump";
                case NodeType.Question:
                default:
                    return "../Question/Question";
            }
        }

        private bool ViewExists(string name)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, name, null);
            return (result.View != null);
        }

        [HttpGet]
        [Route("question/direct/{pathwayId}/{age?}/{pathwayTitle}/{answers?}")]
        public async Task<ActionResult> Direct(string pathwayId, int? age, string pathwayTitle,
            [ModelBinder(typeof (IntArrayModelBinder))] int[] answers) {

            if (!_directLinkingFeature.IsEnabled) {
                return HttpNotFound();
            }

            var resultingModel = await DeriveJourneyView(pathwayId, age, pathwayTitle, answers);
            var viewName = DetermineViewName(resultingModel);
            return View(viewName, resultingModel);
        }

        [HttpGet]
        [Route("question/outcomedetail/{pathwayId}/{age?}/{pathwayTitle}/{answers?}")]
        public async Task<ActionResult> OutcomeDetail(string pathwayId, int? age, string pathwayTitle,
            [ModelBinder(typeof(IntArrayModelBinder))] int[] answers)
        {

            var journeyViewModel = await DeriveJourneyView(pathwayId, age, pathwayTitle, answers);
            var viewName = DetermineViewName(journeyViewModel);
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
            var journeyViewModel = BuildJourneyViewModel(pathwayId, age, pathwayTitle);

            var pathway = JsonConvert.DeserializeObject<Pathway>(await _restfulHelper.GetAsync(_configuration.GetBusinessApiPathwayUrl(pathwayId)));
            if (pathway == null) return null;

            var derivedAge = journeyViewModel.UserInfo.Demography.Age == -1 ? pathway.MinimumAgeInclusive : journeyViewModel.UserInfo.Demography.Age;
            var newModel = new JustToBeSafeViewModel
            {
                PathwayId = pathway.Id,
                PathwayNo = pathway.PathwayNo,
                PathwayTitle = pathway.Title,
                UserInfo = new UserInfo() { Demography = new AgeGenderViewModel { Age = derivedAge, Gender = pathway.Gender } },
                JourneyJson = journeyViewModel.JourneyJson,
                SymptomDiscriminatorCode = journeyViewModel.SymptomDiscriminatorCode,
                State = JourneyViewModelStateBuilder.BuildState(pathway.Gender, derivedAge),
            };

            newModel.StateJson = JourneyViewModelStateBuilder.BuildStateJson(newModel.State);
            journeyViewModel = (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(newModel)).Item2; //todo refactor tuple away

            var resultingModel = await AnswerQuestions(journeyViewModel, answers);
            return resultingModel;
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "PreviousQuestion")]
        public async Task<ActionResult> PreviousQuestion(JourneyViewModel model) {
            ModelState.Clear();

            var url = _configuration.GetBusinessApiQuestionByIdUrl(model.PathwayId, model.Journey.Steps.Last().QuestionId);
            var response = await _restfulHelper.GetAsync(url);
            var questionWithAnswers = JsonConvert.DeserializeObject<QuestionWithAnswers>(response);

            return View("Question", _journeyViewModelBuilder.BuildPreviousQuestion(questionWithAnswers, model));
        }

        private async Task<QuestionWithAnswers> GetNextNode(JourneyViewModel model) {
            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);
            var request = new HttpRequestMessage {
                Content =
                    new StringContent(JsonConvert.SerializeObject(answer.Title), Encoding.UTF8, "application/json")
            };
            var serialisedState = HttpUtility.UrlEncode(model.StateJson);
            var businessApiNextNodeUrl = _configuration.GetBusinessApiNextNodeUrl(model.PathwayId, model.Id, serialisedState);
            var response = await _restfulHelper.PostAsync(businessApiNextNodeUrl, request);
            if (!response.IsSuccessStatusCode)
                throw new Exception(string.Format("Problem posting {0} to {1}.", JsonConvert.SerializeObject(answer.Title), businessApiNextNodeUrl));

            return JsonConvert.DeserializeObject<QuestionWithAnswers>(await response.Content.ReadAsStringAsync());
        }

        private async Task<JourneyViewModel> AnswerQuestions(JourneyViewModel model, int[] answers) {
            if (answers == null)
                return null;

            var queue = new Queue<int>(answers);
            while (queue.Any()) {
                var answer = queue.Dequeue();
                model = await AnswerQuestion(model, answer);
            }
            return model;
        }

        private async Task<JourneyViewModel> AnswerQuestion(JourneyViewModel model, int answer) {
            if (answer < 0 || answer >= model.Answers.Count)
                throw new ArgumentOutOfRangeException(
                    string.Format("The answer index '{0}' was not found within the range of answers: {1}", answer,
                        string.Join(", ", model.Answers.Select(a => a.Title))));

            model.SelectedAnswer = JsonConvert.SerializeObject(model.Answers.First(a => a.Order == answer + 1));
            var result = (ViewResult) await Question(model);

            return result.Model is OutcomeViewModel ? (OutcomeViewModel)result.Model : (JourneyViewModel)result.Model;
        }

        private static JourneyViewModel BuildJourneyViewModel(string pathwayId, int? age, string pathwayTitle) {
            return new JourneyViewModel {
                NodeType = NodeType.Pathway,
                PathwayId = pathwayId,
                PathwayTitle = pathwayTitle,
                UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Age = age ?? -1 }}
            };
        }

        private readonly IJourneyViewModelBuilder _journeyViewModelBuilder;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
        private readonly IDirectLinkingFeature _directLinkingFeature;
        private readonly IAuditLogger _auditLogger;
        private readonly IMappingEngine _mappingEngine;
        }
}