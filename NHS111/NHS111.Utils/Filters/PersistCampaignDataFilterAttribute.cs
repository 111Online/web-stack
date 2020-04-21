using NHS111.Models.Models.Web;
using System;
using System.Web.Mvc;

namespace NHS111.Utils.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PersistCampaignDataFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result == null)
                return;

            var model = result.Model as JourneyViewModel;
            if (model == null)
                return;

            var campaign = filterContext.RequestContext.HttpContext.Request.Params["utm_campaign"];
            if (string.IsNullOrEmpty(campaign)) return;

            model.Campaign = campaign;
            model.Source = filterContext.RequestContext.HttpContext.Request.Params["utm_source"];
        }
    }
}
