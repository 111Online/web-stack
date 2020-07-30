using NHS111.Utils.Attributes;
using System.Web.Mvc;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class HelpController : Controller
    {
        public ActionResult Cookies()
        {
            return View();
        }
        public ActionResult Accessibility()
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

        public ActionResult Language()
        {
            return View();
        }


    }
}