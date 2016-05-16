using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Utils.Attributes;
using NHS111.Utils.Parser;
using NHS111.Web.Presentation.Builders;

namespace NHS111.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;
    using Models.Models.Domain;

    [LogHandleErrorForMVC]
    public class QuestionController : Controller
    {
        private readonly IQuestionViewModelBuilder _questionViewModelBuilder;
        private readonly IJustToBeSafeFirstViewModelBuilder _justToBeSafeFirstViewModelBuilder;


        public QuestionController(IQuestionViewModelBuilder questionViewModelBuilder, IJustToBeSafeFirstViewModelBuilder justToBeSafeFirstViewModelBuilder)
        {
            _questionViewModelBuilder = questionViewModelBuilder;
            _justToBeSafeFirstViewModelBuilder = justToBeSafeFirstViewModelBuilder;
        }

        [HttpPost]
        public async Task<ActionResult> Search()
        {
            return View("Search");
        }

        [HttpPost]
        public async Task<JsonResult> AutosuggestPathways(string input)
        {
            return Json(await _questionViewModelBuilder.BuildSearch(input));
        }

        [HttpPost]
        public ActionResult Gender(JourneyViewModel model)
        {
            return View(_questionViewModelBuilder.BuildGender(model));
        }

        [HttpGet]
        public async Task<ActionResult> GenderDirect(string pathwayTitle)
        {
            var model = await _questionViewModelBuilder.BuildGender(PathwayTitleUriParser.Parse(pathwayTitle));
            if (model.PathwayNo == string.Empty) return Redirect(Url.RouteUrl(new { controller = "Question", action = "Home" }));
            return View("Gender", model);
        }
        
        [HttpGet]
        public ActionResult Home()
        {
            return View("Search");
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "Question")]
        public async Task<ActionResult> Question(JourneyViewModel model)
        {
            ModelState.Clear();
            var next = await _questionViewModelBuilder.BuildQuestion(model);
            
            return View(next.Item1, next.Item2);
        }

        [HttpGet]
        [Route("question/direct/{pathwayId}/{age?}/{pathwayTitle}/{answers?}")]
        public async Task<ActionResult> Direct(string pathwayId, int? age, string pathwayTitle, [ModelBinder(typeof(IntArrayModelBinder))] int[] answers) {

            var journeyViewModel = BuildJourneyViewModel(pathwayId, age, pathwayTitle);

            var action = await _questionViewModelBuilder.ActionSelection(journeyViewModel);
            if (action == null) return Redirect(Url.RouteUrl(new { controller = "Question", action = "Home" }));

            var viewName = action.Item1;
            var model = action.Item2;

            await AnswerQuestions(model, answers);

            return View(viewName, model);
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
                throw new ArgumentOutOfRangeException(string.Format("The answer index '{0}' was not found within the range of answers: {1}", answer, string.Join(", ", model.Answers.Select(a => a.Title))));

            model.SelectedAnswer = Newtonsoft.Json.JsonConvert.SerializeObject(model.Answers[answer]);
            var result = (ViewResult) await Question(model);

            return (JourneyViewModel)result.Model;
        }

        private static JourneyViewModel BuildJourneyViewModel(string pathwayId, int? age, string pathwayTitle) {
            return new JourneyViewModel {
                NodeType = NodeType.Pathway,
                PathwayId = pathwayId,
                PathwayTitle = pathwayTitle,
                UserInfo = new UserInfo {Age = age ?? -1}
            };
        }

        [HttpPost]
        [ActionName("Navigation")]
        [MultiSubmit(ButtonName = "PreviousQuestion")]
        public async Task<ActionResult> PreviousQuestion(JourneyViewModel model)
        {
            ModelState.Clear();
            return View("Question", await _questionViewModelBuilder.BuildPreviousQuestion(model));
        }
    }

    public class IntArrayModelBinder
        : DefaultModelBinder {

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null || string.IsNullOrEmpty(value.AttemptedValue)) {
                return null;
            }

            return value
                .AttemptedValue
                .Split(',')
                .Select(int.Parse)
                .ToArray();
        }
    }
}