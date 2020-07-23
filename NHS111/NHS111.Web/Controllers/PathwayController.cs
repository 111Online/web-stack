
namespace NHS111.Web.Controllers
{
    using Models.Models.Web;
    using System.Web.Mvc;

    public class PathwayController : Controller
    {

        [Route("pathway/{pathwayNo}")]
        public ActionResult Get(string pathwayNo, LocationViewModel model)
        {
            model.PathwayNo = pathwayNo.ToUpper();
            return View("../location/location", model);
        }
    }
}