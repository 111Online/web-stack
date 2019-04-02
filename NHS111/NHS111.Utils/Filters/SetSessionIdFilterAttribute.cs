using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHS111.Models.Models.Web;

namespace NHS111.Utils.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SetSessionIdFilterAttribute : ActionFilterAttribute
    {
        private const string SessionCookieName = "nhs111-session-id";

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.HttpContext.Response.HeadersWritten)
                return;

            var param = filterContext.ActionParameters.Values.FirstOrDefault(p => p is LocationViewModel);

            var model = param as JourneyViewModel;
            if (model == null)
                return;

            if (!filterContext.HttpContext.Request.Cookies.AllKeys.Contains(SessionCookieName))
            {
                model.SessionId = Guid.NewGuid();
                var cookie = new HttpCookie(SessionCookieName, model.SessionId.ToString())
                {
                    Secure = true,
                    Expires = DateTime.Now.AddHours(4) //expire within 4 hours?

                };
                filterContext.HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                var sessionId = filterContext.HttpContext.Request.Cookies[SessionCookieName];
                model.SessionId = Guid.Parse(sessionId.Value);
            }
        }
    }
}
