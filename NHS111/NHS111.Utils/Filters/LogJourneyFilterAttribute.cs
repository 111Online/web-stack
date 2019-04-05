
using System.Linq;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Utils.Filters
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Web.Mvc;
    using Models.Models.Web;
    using Models.Models.Web.Logging;
    using Newtonsoft.Json;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LogJourneyFilterAttribute : ActionFilterAttribute
    {
        private static IRestClient _restClient;

        public LogJourneyFilterAttribute(IRestClient restClientLogJourneyFilter)
        {
            _restClient = restClientLogJourneyFilter;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result == null)
                return;

            var model = result.Model as JourneyViewModel;
            if (model == null)
                return;

           var pageName = !string.IsNullOrEmpty(result.ViewName) ? result.ViewName : string.Format("{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);

            LogAudit(model, pageName);
        }

        private static void LogAudit(JourneyViewModel model, string pageName)
        {
            var auditEntry = model.ToAuditEntry();
            auditEntry.Page = pageName;

            var url = ConfigurationManager.AppSettings["LoggingServiceApiAuditUrl"];
            
            var request = new JsonRestRequest(url, Method.POST);
            request.AddJsonBody(auditEntry);
            _restClient.ExecutePostTaskAsync(request);
        }
    }

    public static class JourneyViewModelExtensions
    {

        private static readonly string CampaignTestingId = "NHS111Testing";
        private static readonly Guid CampaignTestingJourneyId = new Guid("11111111111111111111111111111111");

        public static AuditEntry ToAuditEntry(this JourneyViewModel model)
        {
            var audit = new AuditEntry {
                SessionId = GetSessionId(model.Campaign, model.SessionId),
                JourneyId = model.JourneyId != Guid.Empty ? model.JourneyId.ToString() : null,
                Campaign = model.Campaign,
                CampaignSource = model.Source,
                Journey = model.JourneyJson,
                PathwayId = model.PathwayId,
                PathwayTitle = model.PathwayTitle,
                State = model.StateJson,
                DxCode = model is OutcomeViewModel ? model.Id : "",
                Age = model.UserInfo.Demography != null ? model.UserInfo.Demography.Age : (int?)null,
                Gender = model.UserInfo.Demography != null ? model.UserInfo.Demography.Gender : string.Empty,
                Search = model.EntrySearchTerm
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

        private static Guid GetSessionId(string campaign, Guid sessionId)
        {
            return campaign == CampaignTestingId ? CampaignTestingJourneyId : sessionId;
        }
    }
}