using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NHS111.Web.Controllers
{
    public class HelpController : Controller
    {
        // GET: Help
        public ActionResult Cookies()
        {
            return View();
        }
    }
}