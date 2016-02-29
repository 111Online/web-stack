using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class OutcomeController : Controller
    {
        private readonly IOutcomeViewModelBuilder _outcomeViewModelBuilder;
        private readonly IDOSBuilder _dosBuilder;

        public OutcomeController(IOutcomeViewModelBuilder outcomeViewModelBuilder, IDOSBuilder dosBuilder)
        {
            _outcomeViewModelBuilder = outcomeViewModelBuilder;
            _dosBuilder = dosBuilder;
        }

        [HttpPost]
        public async Task<JsonResult> SearchSurgery(string input)
        {
            return Json((await _outcomeViewModelBuilder.SearchSurgeryBuilder(input))
                .SurgeryViewModel.Surgeries);
        }



        [HttpPost]
        [ActionName("ServiceDetails")]
        [MultiSubmit(ButtonName = "DosResults")]
        public async Task<ActionResult> DosResults(OutcomeViewModel model)
        {
            var viewModel = await _dosBuilder.DosResultsBuilder(model);
            return View("../DOS/DosResults", viewModel);
        }

        [HttpPost]
        [ActionName("ServiceDetails")]
        [MultiSubmit(ButtonName = "DispositionNo2")]
        public async Task<ActionResult> DispositionNo2(OutcomeViewModel model) //TODO this is realyl ugly, bad code duplication, rethink it
        {
            return await DispositionNo(model);
        }


        [HttpPost]
        public async Task<ActionResult> Emergency()
        {
            return View();
        }

        [HttpPost]
        [ActionName("DispositionSelection")]
        [MultiSubmit(ButtonName = "DispositionNo")]
        public async Task<ActionResult> DispositionNo(OutcomeViewModel model)
        {
            model = await _outcomeViewModelBuilder.PersonalDetailsBuilder(model);
            return View("DispositionNo", model);
        }

        //[HttpPost]
        //[ActionName("UserInfo")]
        //[MultiSubmit(ButtonName = "AddressInfoHome")]
        //public ActionResult AddressInfoHome(OutcomeViewModel model)
        //{
        //    ViewBag.AddressType = "AddressInfoHome";
        //    return View("AddressInfo", model);
        //}

        //[HttpPost]
        //[ActionName("UserInfo")]
        //[MultiSubmit(ButtonName = "AddressInfoCurrent")]
        //public ActionResult AddressInfoCurrent(OutcomeViewModel model)
        //{
        //    ViewBag.AddressType = "AddressInfoCurrent";
        //    return View("AddressInfo", model);
        //}

        [HttpPost]
        [ActionName("ServiceDetails")]
        [MultiSubmit(ButtonName = "PostCodeSearch")]
        public async Task<ActionResult> PostCodeSearch(OutcomeViewModel model)
        {
            model = await _outcomeViewModelBuilder.PostCodeSearchBuilder(model);
            return View("PersonalDetails", model);
        }

       
    }
}