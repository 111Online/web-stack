using System.Web.Mvc;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Configuration;
using NHS111.Web.Presentation.Filters;
using RestSharp;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class HelpController : Controller
    {
        public ActionResult Cookies()
        {
            return View();
        }
        
        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }
        
        public ActionResult Browsers()
        {
            return View();
        }
    }
}