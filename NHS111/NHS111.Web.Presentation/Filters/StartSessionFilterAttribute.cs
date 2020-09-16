using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Logging;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NHS111.Web.Presentation.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class StartSessionFilterAttribute : ActionFilterAttribute
    {
        public static readonly string SessionCookieName = "nhs111-session-id";
        private readonly IAuditLogger _auditLogger;

        public StartSessionFilterAttribute(IAuditLogger auditLogger)
        {
            _auditLogger = auditLogger;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Response.HeadersWritten)
                return;

            var model = filterContext.ActionParameters.Values.OfType<JourneyViewModel>().FirstOrDefault() ?? new JourneyViewModel();
            var hasCookie = filterContext.HttpContext.Request.Cookies.AllKeys.Contains(SessionCookieName);
            var hasSessionInModel = model.SessionId != Guid.Empty;

            if (hasCookie && hasSessionInModel)
                return;

            // When the user first lands on the site, store browser info. BrowserInfo is a wrapper that lets us increase accuracy manually.
            var browserInfo = new BrowserInfo(filterContext.HttpContext.Request);
            var pageName = string.Format("{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);

            if (!hasCookie && !hasSessionInModel)
            {
                model.SessionId = Guid.NewGuid();
                var secureCookies = true;

#if DEBUG
                secureCookies = false;
#endif

                var cookie = new HttpCookie(SessionCookieName, model.SessionId.ToString())
                {
                    Secure = secureCookies,
                    Expires = DateTime.Now.AddHours(4), //expire within 4 hours?
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = true
                };
                filterContext.HttpContext.Response.Cookies.Add(cookie);

                // Check if the referrer is NHS App, ideally should use the cookie but that is not set till after this. 
                // That's not an issue as the app will always be using the query string on session start.
                var isNHSApp = filterContext.HttpContext.Request.QueryString["utm_medium"] == "nhs app";
                var isSMSReferral = !string.IsNullOrEmpty(filterContext.HttpContext.Request.QueryString["d"]);
                var referrer = isNHSApp ? "NHS App" : isSMSReferral ? "SMS Text" : browserInfo.Referer;
                var campaign = filterContext.HttpContext.Request.QueryString["Campaign"];
                model.Campaign = campaign;

                _auditLogger.LogEvent(model, EventType.Browser, browserInfo.Browser, pageName);
                _auditLogger.LogEvent(model, EventType.BrowserVersion, browserInfo.MajorVersionString, pageName);
                _auditLogger.LogEvent(model, EventType.OperatingSystem, browserInfo.Platform, pageName);
                _auditLogger.LogEvent(model, EventType.DeviceType, browserInfo.DeviceType, pageName);
                _auditLogger.LogEvent(model, EventType.Referer, referrer, pageName);
            }
            else if (hasCookie)
            {
                var sessionId = filterContext.HttpContext.Request.Cookies[SessionCookieName];
                model.SessionId = Guid.Parse(sessionId.Value);
            }

            if (!string.IsNullOrEmpty(filterContext.HttpContext.Request.QueryString["d"]))
            {
                var covidValue = filterContext.HttpContext.Request.QueryString["d"];
                _auditLogger.LogEvent(model, EventType.Clicked, string.Format("External COVID-19 SMS Text : {0}", covidValue), pageName);
            }
        }
    }
}
