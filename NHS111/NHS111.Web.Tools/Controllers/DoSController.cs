using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NHS111.Web.Tools.Controllers
{
    [LogHandleErrorForMVC]
    public class DoSController : Controller
    {
        private readonly IDOSBuilder _dosBuilder;
        private readonly ISurgeryBuilder _surgeryBuilder;

        public DoSController(IDOSBuilder dosBuilder, ISurgeryBuilder surgeryBuilder)
        {
            _dosBuilder = dosBuilder;
            _surgeryBuilder = surgeryBuilder;
        }

        [HttpGet]
        public ActionResult Search()
        {
            var model = new DosViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> FillServiceDetails(DosViewModel model)
        {
            var dosView = new DosViewModel
            {
                CheckCapacitySummaryResultList = (await _dosBuilder.FillCheckCapacitySummaryResult(model)),
                DosServicesByClinicalTermResultList = (await _dosBuilder.FillDosServicesByClinicalTermResult(model))
            };
            return PartialView("_DoSComparisionResultsView", dosView);
        }

        [HttpPost]
        public async Task<JsonResult> SearchSurgery(string input)
        {
            return Json((await _surgeryBuilder.SearchSurgeryBuilder(input)));
        }
    }
}