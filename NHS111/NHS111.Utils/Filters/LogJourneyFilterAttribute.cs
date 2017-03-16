
using System.Linq;
using NHS111.Models.Models.Web.FromExternalServices;

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
    using System.Collections.Generic;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LogJourneyFilterAttribute : ActionFilterAttribute
    {
        private readonly List<string> _manuallyTriggeredAuditList = new List<string>
        {
            "ServiceDetails",
            "ServiceList",
            "Confirmation"
        };

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result == null)
                return;

            var model = result.Model as JourneyViewModel;
            if (model == null) 
                return;

            if (filterContext.RouteData.Values["controller"].Equals("Outcome") && _manuallyTriggeredAuditList.Contains(filterContext.RouteData.Values["action"]))
                return; //we don't want to audit where audit has already been maually triggerred in code

                LogAudit(model);
        }

        private static void LogAudit(JourneyViewModel model) {
            var url = ConfigurationManager.AppSettings["LoggingServiceUrl"];
            var rest = new RestfulHelper();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url)) {
                Content = new StringContent(JsonConvert.SerializeObject(model.ToAuditEntry()))
            };
            rest.PostAsync(url, httpRequestMessage);
        }
    }

    public static class JourneyViewModelExtensions {
        public static AuditEntry ToAuditEntry(this JourneyViewModel model) {
            var audit = new AuditEntry {
                SessionId = model.SessionId,
                JourneyId = model.JourneyId != Guid.Empty ? model.JourneyId.ToString() : null,
                Journey = model.JourneyJson,
                PathwayId = model.PathwayId,
                PathwayTitle = model.PathwayTitle,
                State = model.StateJson,
                DxCode = model is OutcomeViewModel ? model.Id : ""
            };
            AddLatestJourneyStepToAuditEntry(model.Journey, audit);
            
            return audit;
        }

        private static void AddLatestJourneyStepToAuditEntry(Journey journey, AuditEntry auditEntry)
        {
            if (journey == null || journey.Steps == null || journey.Steps.Count <= 0) return;

            var step = journey.Steps.Last();
            if (step.Answer != null)
            {
                auditEntry.AnswerTitle = step.Answer.Title;
                auditEntry.AnswerOrder = step.Answer.Order.ToString();
            }

            auditEntry.QuestionId = step.QuestionId;
            auditEntry.QuestionNo = step.QuestionNo;
            auditEntry.QuestionTitle = step.QuestionTitle;
        }
    }
}
