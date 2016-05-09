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
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;

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
        public async Task<ActionResult> QuestionDirect(string pathwayId, int? age, string pathwayTitle)
        {
            var journeyViewModel = new JourneyViewModel()
            {
                NodeType = NodeType.Pathway,
                PathwayId = pathwayId,
                PathwayTitle = pathwayTitle,
                UserInfo = new UserInfo() { Age = age ?? -1 }
            };
            var model = await _questionViewModelBuilder.ActionSelection(journeyViewModel);
            if (model == null) return Redirect(Url.RouteUrl(new { controller = "Question", action = "Home" }));
            return View(model.Item1, model.Item2);
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
}