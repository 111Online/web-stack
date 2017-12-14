using System.Web;

namespace NHS111.Utils.Filters {
    using System;
    using System.Web.Mvc;
    using Models.Models.Web;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SetSessionIdFilterAttribute : ActionFilterAttribute {
        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            var result = filterContext.Result as ViewResultBase;
            if (result == null)
                return;

            var model = result.Model as JourneyViewModel;
            if (model == null) 
                return;

            var sessionIdCookie = filterContext.RequestContext.HttpContext.Request.Cookies["nhs111-session-id"];
            if (sessionIdCookie == null)
            {
                sessionIdCookie = new HttpCookie("nhs111-session-id")
                {
                    Value = filterContext.RequestContext.HttpContext.Request.AnonymousID,
                    Expires = DateTime.Now.AddHours(72)
                };
                filterContext.RequestContext.HttpContext.Response.Cookies.Add(sessionIdCookie);
            }
            model.SessionId = Guid.Parse(sessionIdCookie.Value);
        }
    }
}