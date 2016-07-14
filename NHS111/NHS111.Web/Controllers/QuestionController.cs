
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
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Web;
    using AutoMapper;
    using Models.Models.Domain;
    using Newtonsoft.Json;
    using Presentation.ModelBinders;
    using Utils.Helpers;
    using IConfiguration = Presentation.Configuration.IConfiguration;

    [LogHandleErrorForMVC]
    public class QuestionController
        : Controller {

        public QuestionController(IJourneyViewModelBuilder journeyViewModelBuilder, IRestfulHelper restfulHelper,
            IConfiguration configuration, IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder, IMappingEngine mappingEngine) {
            _journeyViewModelBuilder = journeyViewModelBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
            _mappingEngine = mappingEngine;
        }

        [HttpPost]
        public async Task<ActionResult> Search() {
            return View("Search");
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
            var model = new JourneyViewModel() {PathwayNo = pathwayNo};

            if (model.PathwayNo == string.Empty)
                return Redirect(Url.RouteUrl(new {controller = "Question", action = "Home"}));
            return View("Gender", model);
        }

        [HttpGet]
        public ActionResult Home() {
            return View("Search");
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

        private async Task<JourneyViewModel> GetNextJourneyViewModel(JourneyViewModel model) {
            var nextNode = await GetNextNode(model);
            return await _journeyViewModelBuilder.Build(model, nextNode);
        }

        private string DetermineViewName(JourneyViewModel model) {
            switch (model.NodeType) {
                case NodeType.Outcome:
                    if (OutcomeGroup.Call999.Equals(model.OutcomeGroup))
                        return "../Outcome/Emergency";

                    if (OutcomeGroup.HomeCare.Equals(model.OutcomeGroup))
                        return "../Outcome/HomeCare";

                    return (OutcomeGroup.AccidentAndEmergency.Equals(model.OutcomeGroup))
                        ? "../Outcome/Disposition2"
                        : "../Outcome/Disposition";
                case NodeType.DeadEndJump:
                    return "../Question/DeadEndJump";
                case NodeType.Question:
                default:
                    return "../Question/Question";
            }
        }

        [HttpGet]
        [Route("question/direct/{pathwayId}/{age?}/{pathwayTitle}/{answers?}")]
        public async Task<ActionResult> Direct(string pathwayId, int? age, string pathwayTitle,
            [ModelBinder(typeof (IntArrayModelBinder))] int[] answers) {

            if (_configuration.IsPublic) {
                return HttpNotFound();
            }

            //the below is copied from refactored code. Suggest removing once JTBS code is refactored away.
            var journeyViewModel = BuildJourneyViewModel(pathwayId, age, pathwayTitle);

            var pathway = JsonConvert.DeserializeObject<Pathway>(await _restfulHelper.GetAsync(_configuration.GetBusinessApiPathwayUrl(pathwayId)));
            if (pathway == null) return null;

            var derivedAge = journeyViewModel.UserInfo.Age == -1 ? pathway.MinimumAgeInclusive : journeyViewModel.UserInfo.Age;
            var newModel = new JustToBeSafeViewModel
            {
                PathwayId = pathway.Id,
                PathwayNo = pathway.PathwayNo,
                PathwayTitle = pathway.Title,
                UserInfo = new UserInfo() { Age = derivedAge, Gender = pathway.Gender },
                JourneyJson = journeyViewModel.JourneyJson,
                SymptomDiscriminator = journeyViewModel.SymptomDiscriminator,
                State = JourneyViewModelStateBuilder.BuildState(pathway.Gender, derivedAge),
            };

            newModel.StateJson = JourneyViewModelStateBuilder.BuildStateJson(newModel.State);
            journeyViewModel = (await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(newModel)).Item2; //todo refactor tuple away

            await AnswerQuestions(journeyViewModel, answers);
            var viewName = DetermineViewName(journeyViewModel);
            return View(viewName, journeyViewModel);
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

        private async Task AnswerQuestions(JourneyViewModel model, int[] answers) {
            if (answers == null)
                return;

            var queue = new Queue<int>(answers);
            while (queue.Any()) {
                var answer = queue.Dequeue();
                model = await AnswerQuestion(model, answer);
            }
        }

        private async Task<JourneyViewModel> AnswerQuestion(JourneyViewModel model, int answer) {
            if (answer < 0 || answer >= model.Answers.Count)
                throw new ArgumentOutOfRangeException(
                    string.Format("The answer index '{0}' was not found within the range of answers: {1}", answer,
                        string.Join(", ", model.Answers.Select(a => a.Title))));

            model.SelectedAnswer = JsonConvert.SerializeObject(model.Answers.First(a => a.Order == answer + 1));
            var result = (ViewResult) await Question(model);

            return (JourneyViewModel) result.Model;
        }

        private static JourneyViewModel BuildJourneyViewModel(string pathwayId, int? age, string pathwayTitle) {
            return new JourneyViewModel {
                NodeType = NodeType.Pathway,
                PathwayId = pathwayId,
                PathwayTitle = pathwayTitle,
                UserInfo = new UserInfo {Age = age ?? -1}
            };
        }

        private readonly IJourneyViewModelBuilder _journeyViewModelBuilder;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;
        private readonly IMappingEngine _mappingEngine;
    }
}