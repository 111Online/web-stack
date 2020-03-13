using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("COVID-19")]
        [Route("service/COVID-19")]
        //Special route for Covid direct link from other services to tidy up..
        public ActionResult StartCovidJourney()
        {
            return View("AboutCovid");
        }

        [HttpGet]
        [Route("{pathwayNumber}/{sessionId}/{digitalTitle}/about")] // Old link, kept so it doesn't 404
        public ActionResult RedirectSessionCovidJourney()
        {
            return RedirectPermanent("/service/COVID-19");
        }
    }
}