using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.Logging;
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

            var model = filterContext.ActionParameters.Values.OfType<JourneyViewModel>().FirstOrDefault();
            
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

                // When the user first lands on the site, store browser info
                var browserInfo = filterContext.HttpContext.Request.Browser;

                var pageName = string.Format("{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);
                _auditLogger.LogEvent(model, EventType.Browser, browserInfo.Browser, pageName);
                _auditLogger.LogEvent(model, EventType.BrowserVersion, browserInfo.MajorVersion.ToString(), pageName);
                _auditLogger.LogEvent(model, EventType.OperatingSystem,browserInfo.Platform, pageName);
            }
            else
            {
                var sessionId = filterContext.HttpContext.Request.Cookies[SessionCookieName];
                model.SessionId = Guid.Parse(sessionId.Value);
            }
        }
    }
}
