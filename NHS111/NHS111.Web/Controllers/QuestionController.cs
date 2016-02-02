using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;

namespace NHS111.Web.Controllers
{

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


        public async Task<ActionResult> Gender(JourneyViewModel model)
        {
            var journey = await _questionViewModelBuilder.BuildGender(model);

            
            if (journey == null) Response.RedirectToRoute("Default", new RouteValueDictionary() { {"controller", "Question"}, {"action", "Home"} });
            
            return View(journey);
        }

        [HttpPost]
        [ActionName("SliderAction")]
        [MultiSubmit(ButtonName = "Slider")]
        public async Task<ActionResult> Slider(JourneyViewModel model)
        {
            ModelState.Clear();
            var sliderModel = await _questionViewModelBuilder.BuildSlider(model);
            return View("Slider", sliderModel);
        }

        [HttpPost]
        [ActionName("SliderAction")]
        [MultiSubmit(ButtonName = "JustToBeSafe")]
        public async Task<ActionResult> JustToBeSafe(JourneyViewModel model)
        {
            var sliderModel = await _questionViewModelBuilder.BuildSlider(model);
            var justToBeSafeViewModel = new JustToBeSafeViewModel
            {
                PathwayId = sliderModel.PathwayId,
                PathwayNo = sliderModel.PathwayNo,
                PathwayTitle = sliderModel.PathwayTitle,
                UserInfo = sliderModel.UserInfo
            };
            var next = await _justToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(justToBeSafeViewModel);
            return View(next.Item1, next.Item2);
        }

        [HttpGet]
        public ActionResult Home()
        {
            return View();
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