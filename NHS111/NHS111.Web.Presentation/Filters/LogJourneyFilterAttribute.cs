using System;
using System.Linq;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Logging;
using NHS111.Web.Presentation.Logging;

namespace NHS111.Web.Presentation.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LogJourneyFilterAttribute : ActionFilterAttribute
    {
        private readonly IAuditLogger _auditLogger;

        public LogJourneyFilterAttribute(IAuditLogger auditLogger)
        {
            _auditLogger = auditLogger;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result == null)
                return;

            var model = result.Model as JourneyViewModel;
            if (model == null)
                return;

            var pageName = !string.IsNullOrEmpty(result.ViewName)
                ? result.ViewName
                : string.Format("{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName);

            LogAudit(model, pageName);
        }

        private void LogAudit(JourneyViewModel model, string pageName)
        {
            var auditEntry = model.ToAuditEntry();
            auditEntry.Page = pageName;
            _auditLogger.Log(auditEntry);
        }
    }

    public static class JourneyViewModelExtensions
    {

        private static readonly string CampaignTestingId = "NHS111Testing";
        private static readonly Guid CampaignTestingJourneyId = new Guid("11111111111111111111111111111111");

        public static AuditEntry ToAuditEntry(this JourneyViewModel model)
        {
            var audit = new AuditEntry
            {
                SessionId = GetSessionId(model.Campaign, model.SessionId),
                JourneyId = model.JourneyId != Guid.Empty ? model.JourneyId.ToString() : null,
                Campaign = model.Campaign,
                CampaignSource = model.Source,
                PathwayId = model.PathwayId,
                PathwayTitle = model.PathwayTitle,
                DxCode = model is OutcomeViewModel ? model.Id : "",
                Age = model.UserInfo.Demography != null ? model.UserInfo.Demography.Age : (int?) null,
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