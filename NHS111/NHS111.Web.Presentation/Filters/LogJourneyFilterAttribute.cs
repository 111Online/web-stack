using Newtonsoft.Json;
using NHS111.Models.Mappers.WebMappings;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Logging;
using NHS111.Web.Presentation.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
                State = GetAuditedState(model.StateJson),
                DxCode = model is OutcomeViewModel ? model.Id : "",
                Age = model.UserInfo.Demography != null ? model.UserInfo.Demography.Age : (int?)null,
                Gender = model.UserInfo.Demography != null ? model.UserInfo.Demography.Gender : string.Empty,
                Search = model.EntrySearchTerm,
                PostCodePart = AuditedModelMappers.GetPartialPostcode(model.CurrentPostcode)
            };
            AddLatestJourneyStepToAuditEntry(model.Journey, audit);

            return audit;
        }

        public static AuditEntry ToAuditEntry(this SurveyLinkViewModel model)
        {
            var audit = new AuditEntry
            {
                SessionId = GetSessionId(model.Campaign, model.SessionId),
                JourneyId = Guid.Parse(model.JourneyId) != Guid.Empty ? model.JourneyId : null,
                Campaign = model.Campaign,
                PathwayId = model.EndPathwayNo,
                PathwayTitle = model.EndPathwayTitle,
                DxCode = model.DispositionCode
            };

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

        private static string GetAuditedState(string state)
        {
            if (state == null || string.IsNullOrEmpty(state)) return string.Empty;

            var stateItems = JsonConvert.DeserializeObject<IDictionary<string, string>>(state);
            if (!stateItems.Any()) return string.Empty;

            var auditedState = stateItems
                .Where(i => i.Key.Equals("PATIENT_AGE") || i.Key.Equals("PATIENT_GENDER") || i.Key.Equals("PATIENT_AGEGROUP") || i.Key.Equals("PATIENT_PARTY"))
                .ToDictionary(x => x.Key, x => x.Value);
            var json = JsonConvert.SerializeObject(auditedState);
            return json;
        }
    }
}