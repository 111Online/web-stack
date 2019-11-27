using System.Web.Mvc;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Attributes;
using NHS111.Utils.Filters;
using NHS111.Web.Presentation.Configuration;
using RestSharp;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class HelpController : Controller
    {
        [SetSessionIdFilter]
        public ActionResult Cookies()
        {
            return View();
        }
        
        [SetSessionIdFilter]
        public ActionResult Privacy()
        {
            return View();
        }
        
        [SetSessionIdFilter]
        public ActionResult Terms()
        {
            return View();
        }
        
        [SetSessionIdFilter]
        public ActionResult Browsers()
        {
            return View();
        }
    }
}