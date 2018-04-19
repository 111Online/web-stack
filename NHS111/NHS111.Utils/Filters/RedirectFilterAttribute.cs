using System;
using System.Web.Mvc;
using NHS111.Features;
using NHS111.Models.Models.Web;

namespace NHS111.Utils.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RedirectFilterAttribute : ActionFilterAttribute
    {
        private readonly IRedirectToStartFeature _redirectToStartFeature;
        public RedirectFilterAttribute(IRedirectToStartFeature redirectToStartFeature)
        {
            _redirectToStartFeature = redirectToStartFeature;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!_redirectToStartFeature.IsEnabled) return;

            var result = filterContext.Result as ViewResultBase;
            if (result == null)
                return;

            var model = result.Model as JourneyViewModel;
            if (model == null)
                return;


            if (string.IsNullOrEmpty(model.CurrentPostcode) && model.OutcomeGroup == null)
                filterContext.Result = new RedirectResult(_redirectToStartFeature.RedirectUrl, false);
        }
    }
}
