using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Filters;
using RestSharp;

namespace NHS111.Web.Presentation.Logging {
    using System.Threading.Tasks;
    using Configuration;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Web.Logging;

    public interface IAuditLogger
    {
        void Log(AuditEntry auditEntry);
        void LogDosRequest(OutcomeViewModel model, DosViewModel dosViewModel);
        void LogDosResponse(OutcomeViewModel model, DosCheckCapacitySummaryResult result);
        void LogEventData(JourneyViewModel model, string eventData);
        void LogSelectedService(OutcomeViewModel model);
        void LogItkRequest(OutcomeViewModel model, ITKDispatchRequest itkRequest);
        void LogItkResponse(OutcomeViewModel model, IRestResponse response);
    }

    public class AuditLogger : IAuditLogger {
        
        public AuditLogger(IRestClient restClient, IConfiguration configuration) {
            _restClient = restClient;
            _configuration = configuration;
        }

        public void Log(AuditEntry auditEntry)
        {
            Task.Run(() =>
            {
                var url = _configuration.LoggingServiceApiAuditUrl;
                var request = new JsonRestRequest(url, Method.POST);
                request.AddJsonBody(auditEntry);
                _restClient.ExecuteTaskAsync(request);
            });
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

        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;
    }
}