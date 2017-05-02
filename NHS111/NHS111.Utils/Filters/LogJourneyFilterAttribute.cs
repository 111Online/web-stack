
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
    using System.Web;
    using System.Web.SessionState;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LogJourneyFilterAttribute : ActionFilterAttribute
    {
        private readonly Dictionary<string, List<string>> _manuallyTriggeredAuditList = new Dictionary<string, List<string>>
        {
            { "Outcome", new List<string> { "ServiceDetails", "ServiceList", "PersonalDetails", "Confirmation" } },
            { "PostcodeFirst", new List<string> { "Outcome", "Postcode" } }
        };

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result == null)
                return;

            var model = result.Model as JourneyViewModel;
            if (model == null)
                return;

            var controller = filterContext.RouteData.Values["controller"] as string;
            var action = filterContext.RouteData.Values["action"] as string;
            if (_manuallyTriggeredAuditList.ContainsKey(controller) && _manuallyTriggeredAuditList[controller].Contains(action) || 
                controller.Equals("Question") && result.ViewName == "../PostcodeFirst/Postcode") // don't log when hitting postcode first page
                return; //we don't want to audit where audit has already been manually triggered in code

            var campaign = filterContext.RequestContext.HttpContext.Request.Params["utm_campaign"];
            if (!string.IsNullOrEmpty(campaign))
            {
                if (filterContext.RequestContext.HttpContext.Session != null)
                {
                    filterContext.RequestContext.HttpContext.Session["utm_campaign"] = campaign;
                    filterContext.RequestContext.HttpContext.Session["utm_source"] = filterContext.RequestContext.HttpContext.Request.Params["utm_source"];
                }
            }

            LogAudit(model, filterContext.RequestContext.HttpContext.Session);
        }

        private static void LogAudit(JourneyViewModel model, HttpSessionStateBase session)
        {
            var url = ConfigurationManager.AppSettings["LoggingServiceUrl"];
            var rest = new RestfulHelper();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
            {
                Content = new StringContent(JsonConvert.SerializeObject(model.ToAuditEntry(session)))
            };
            rest.PostAsync(url, httpRequestMessage);
        }
    }

    public static class JourneyViewModelExtensions
    {

        private static readonly string CampaignTestingId = "NHS111Testing";
        private static readonly Guid CampaignTestingJourneyId = new Guid("11111111111111111111111111111111");

        public static AuditEntry ToAuditEntry(this JourneyViewModel model, HttpSessionStateBase session)
        {
            var audit = new AuditEntry {
                SessionId = GetSessionId(session["utm_campaign"] as string, model.SessionId),
                JourneyId = model.JourneyId != Guid.Empty ? model.JourneyId.ToString() : null,
                Campaign = session["utm_campaign"] as string,
                CampaignSource = session["utm_source"] as string,
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

        private static Guid GetSessionId(string campaign, Guid sessionId)
        {
            return campaign == CampaignTestingId ? CampaignTestingJourneyId : sessionId;
        }
    }
}