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

            model.SessionId = Guid.Parse(filterContext.RequestContext.HttpContext.Request.AnonymousID);
        }
    }
}