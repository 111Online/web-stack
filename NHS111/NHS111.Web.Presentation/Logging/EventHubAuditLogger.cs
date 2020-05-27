using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Web.Presentation.Filters;
using RestSharp;

namespace NHS111.Web.Presentation.Logging
{
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;
    using NHS111.Web.Presentation.Configuration;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Web.Logging;

    public class EventHubAuditLogger : IAuditLogger
    {

        private readonly IConfiguration _configuration;
        private readonly EventHubProducerClient _eventHubProducerClient;

        public EventHubAuditLogger(IConfiguration configuration)
        {
            _configuration = configuration;
            if (_configuration.AuditEventHubEnabled)
            {
                _eventHubProducerClient = new EventHubProducerClient(_configuration.AuditEventHubConnectionString);
            }
        }

        public void Log(AuditEntry auditEntry)
        {
            if (_configuration.AuditEventHubEnabled)
            {
                HostingEnvironment.QueueBackgroundWorkItem(ct => LogToEventHub(auditEntry, ct));
            }
        }

        private async Task LogToEventHub(AuditEntry auditEntry, CancellationToken ct = default(CancellationToken))
        {
            var batch = await _eventHubProducerClient.CreateBatchAsync(ct);
            var data = new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(auditEntry)));
            batch.TryAdd(data);
            await _eventHubProducerClient.SendAsync(batch, ct);
        }

        public void LogDosRequest(OutcomeViewModel model, DosViewModel dosViewModel)
        {
            var audit = model.ToAuditEntry();
            var auditedDosViewModel = Mapper.Map<AuditedDosRequest>(dosViewModel);
            audit.DosRequest = JsonConvert.SerializeObject(auditedDosViewModel);
            Log(audit);
        }

        public void LogDosResponse(OutcomeViewModel model, DosCheckCapacitySummaryResult result)
        {
            var audit = model.ToAuditEntry();
            var auditedDosResponse = Mapper.Map<AuditedDosResponse>(result);
            audit.DosResponse = JsonConvert.SerializeObject(auditedDosResponse);
            Log(audit);
        }

        public void LogSelectedService(OutcomeViewModel model)
        {
            LogEventData(model, string.Format("User selected service '{0}' ({1})", model.SelectedService.Name, model.SelectedService.Id));
        }

        public void LogEventData(JourneyViewModel model, string eventData)
        {
            var audit = model.ToAuditEntry();
            audit.EventData = eventData;
            Log(audit);
        }

        public void LogEvent(JourneyViewModel model, EventType eventKey, string eventValue, string page = "")
        {
            var audit = model.ToAuditEntry();
            audit.EventKey = eventKey;
            audit.EventValue = eventValue;
            audit.Page = page;
            Log(audit);
        }

        public void LogItkRequest(OutcomeViewModel model, ITKDispatchRequest itkRequest)
        {
            var audit = model.ToAuditEntry();
            var auditedItkRequest = Mapper.Map<AuditedItkRequest>(itkRequest);
            audit.ItkRequest = JsonConvert.SerializeObject(auditedItkRequest);
            Log(audit);
        }

        public void LogItkResponse(OutcomeViewModel model, IRestResponse response)
        {
            var audit = model.ToAuditEntry();
            var auditedItkResponse = Mapper.Map<AuditedItkResponse>(response);
            audit.ItkResponse = JsonConvert.SerializeObject(auditedItkResponse);
            Log(audit);
        }

        public void LogPrimaryCareReason(OutcomeViewModel model, string reason)
        {

            if (string.IsNullOrEmpty(reason))
                return;

            var audit = model.ToAuditEntry();

            switch (reason)
            {
                case "cannot-get-appt":
                    audit.EventData = "Patient cannot get a GP appointment";
                    break;
                case "away-from-home":
                    audit.EventData = "Patient is away from home";
                    break;
                case "no-gp":
                    audit.EventData = "Patient has no GP";
                    break;
            }
            Log(audit);
        }

        public void LogSurveyInterstitial(SurveyLinkViewModel model)
        {
            var audit = model.ToAuditEntry();
            audit.Page = "Survey Interstitial";
            Log(audit);
        }
    }
}