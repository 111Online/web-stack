﻿using System;
using System.Web;
using System.Web.Mvc;
using NHS111.Models.Models.Web;

namespace NHS111.Web.App_Start
{

    public class ApplicationMediumActionFilter : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ApplicationMediums.SetFromRequest(filterContext);
            filterContext.Controller.ViewBag.Medium = ApplicationMediums.GetFromRequest(filterContext.RequestContext.HttpContext.Request);
        
        }
    }

}