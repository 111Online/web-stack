using System;
using System.Diagnostics;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using NHS111.Web.App_Start;

namespace NHS111.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
            filters.Add(new ApplicationMediumActionFilter());
        }
    }
}

