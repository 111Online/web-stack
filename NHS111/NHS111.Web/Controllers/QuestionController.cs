
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
    using Models.Models.Web.FromExternalServices;
    using Newtonsoft.Json;
    using Presentation.Configuration;
    using Presentation.ModelBinders;
    using Utils.Helpers;

    [LogHandleErrorForMVC]
    public class QuestionController
        : Controller {

        public QuestionController(IJourneyViewModelBuilder journeyViewModelBuilder, IRestfulHelper restfulHelper,
            IConfiguration configuration) {
            _journeyViewModelBuilder = journeyViewModelBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
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
            var nextNode = await GetNextNode(model);
            var nextModel = await _journeyViewModelBuilder.Build(model, nextNode);

            var viewName = DeterminViewName(nextModel);

            return View(viewName, nextNode);
        }

        private string DeterminViewName(JourneyViewModel model) {
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

            var journeyViewModel = BuildJourneyViewModel(pathwayId, age, pathwayTitle);

            await AnswerQuestions(journeyViewModel, answers);
            var viewName = DeterminViewName(journeyViewModel);
            return View(viewName, journeyViewModel);
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "PreviousQuestion")]
        public async Task<ActionResult> PreviousQuestion(JourneyViewModel model) {
            ModelState.Clear();

            var journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
            var url = _configuration.GetBusinessApiQuestionByIdUrl(model.PathwayId, journey.Steps.Last().QuestionId);
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
            var serialisedState = HttpUtility.UrlEncode(JsonConvert.SerializeObject(model.State));
            var businessApiNextNodeUrl = _configuration.GetBusinessApiNextNodeUrl(model.PathwayId, model.Id, serialisedState);
            var response = await _restfulHelper.PostAsync(businessApiNextNodeUrl, request);
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
                NodeType = NodeType.Question, //todo test
                PathwayId = pathwayId,
                PathwayTitle = pathwayTitle,
                UserInfo = new UserInfo {Age = age ?? -1}
            };
        }

        private readonly IJourneyViewModelBuilder _journeyViewModelBuilder;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
    }
}