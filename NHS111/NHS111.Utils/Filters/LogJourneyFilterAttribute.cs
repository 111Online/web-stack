
namespace NHS111.Utils.Filters
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Web.Mvc;
    using Helpers;
    using Models.Models.Web;
    using Models.Models.Web.Logging;
    using Newtonsoft.Json;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LogJourneyFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result == null)
                return;

            var model = result.Model as JourneyViewModel;
            if (model == null) 
                return;

            LogAudit(model);
        }

        private static void LogAudit(JourneyViewModel model) {
            var url = ConfigurationManager.AppSettings["LoggingServiceUrl"];
            var rest = new RestfulHelper();
            var audit = new AuditEntry {
                SessionId = model.SessionId,
                Journey = model.JourneyJson,
                PathwayId = model.PathwayId,
                PathwayTitle = model.PathwayTitle,
                State = model.StateJson
            };
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url)) {
                Content = new StringContent(JsonConvert.SerializeObject(audit))
            };
            rest.PostAsync(url, httpRequestMessage);
        }
    }
}
