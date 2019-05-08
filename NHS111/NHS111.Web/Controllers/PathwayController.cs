
namespace NHS111.Web.Controllers {
    using System.Web.Mvc;
    using Models.Models.Web;

    public class PathwayController
        : Controller {

        [Route("pathway/{pathwayNo}")]
        public ActionResult Get(string pathwayNo, LocationViewModel model) {
            model.PathwayNo = pathwayNo.ToUpper();
            return View("../location/location", model);
        }
    }
}