
namespace NHS111.Web.Controllers {
    using System.Web.Mvc;
    using Models.Models.Web;
    using Utils.Filters;

    public class PathwayController
        : Controller {

        [Route("pathway/{pathwayNo}")]
        [SetSessionIdFilter]
        public ActionResult Get(string pathwayNo, LocationViewModel model) {
            model.PathwayNo = pathwayNo.ToUpper();
            return View("../location/location", model);
        }

        [Route("emergency-prescription")]
        [SetSessionIdFilter]
        public ActionResult GetEP(LocationViewModel model) {
            return Get(_emergencyPrescriptionPathwayNo, model);
        }

        private string _emergencyPrescriptionPathwayNo = "PW1827";
    }
}