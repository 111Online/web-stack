using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.Logging;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Logging;

namespace NHS111.Web.Presentation.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class StartSessionFilterAttribute : ActionFilterAttribute
    {
        private const string SessionCookieName = "nhs111-session-id";
        private readonly IAuditLogger _auditLogger;

        public StartSessionFilterAttribute(IAuditLogger auditLogger)
        {
            _auditLogger = auditLogger;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (filterContext.HttpContext.Response.HeadersWritten)
                return;

            var model = filterContext.ActionParameters.Values.OfType<JourneyViewModel>().FirstOrDefault() ?? new JourneyViewModel();

            if (!filterContext.HttpContext.Request.Cookies.AllKeys.Contains(SessionCookieName))
            {
                model.SessionId = Guid.NewGuid();
                var secureCookies = true;
                
#if DEBUG
                secureCookies = false;
#endif

                var cookie = new HttpCookie(SessionCookieName, model.SessionId.ToString())
                {
                    Secure = secureCookies,
                    Expires = DateTime.Now.AddHours(4) //expire within 4 hours?

                };
                filterContext.HttpContext.Response.Cookies.Add(cookie);

                // When the user first lands on the site, store browser info. BrowserInfo is a wrapper that lets us increase accuracy manually.
                var browserInfo = new BrowserInfo(filterContext.HttpContext.Request);
                var pageName = string.Format("{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);

                _auditLogger.LogEvent(model, EventType.Browser, browserInfo.Browser, pageName);
                _auditLogger.LogEvent(model, EventType.BrowserVersion, browserInfo.MajorVersionString, pageName);
                _auditLogger.LogEvent(model, EventType.OperatingSystem,browserInfo.Platform, pageName);
                _auditLogger.LogEvent(model, EventType.DeviceType, browserInfo.DeviceType, pageName);
                _auditLogger.LogEvent(model, EventType.Referer, browserInfo.Referer, pageName);
            }
            else
            {
                var sessionId = filterContext.HttpContext.Request.Cookies[SessionCookieName];
                model.SessionId = Guid.Parse(sessionId.Value);
            }
        }
    }
}
