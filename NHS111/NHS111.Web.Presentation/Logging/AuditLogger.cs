
using System.Web;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Utils.Filters;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Web.Presentation.Logging {
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Configuration;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Web.Logging;
    using Utils.Helpers;

    public interface IAuditLogger
    {
        Task Log(AuditEntry auditEntry);
        Task LogDosRequest(OutcomeViewModel model, DosViewModel dosViewModel);
        Task LogDosResponse(OutcomeViewModel model, DosCheckCapacitySummaryResult result);
        Task LogEventData(JourneyViewModel model, string eventData);
        Task LogSelectedService(OutcomeViewModel model);
        Task LogItkRequest(OutcomeViewModel model, ITKDispatchRequest itkRequest);
        Task LogItkResponse(OutcomeViewModel model, string response);
    }

    public class AuditLogger : IAuditLogger {
        
        public AuditLogger(IRestClient restClient, IConfiguration configuration) {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task Log(AuditEntry auditEntry)
        {
            var url = _configuration.LoggingServiceApiAuditUrl;
            var request = new JsonRestRequest(url, Method.POST);
            request.AddJsonBody(auditEntry);
            await _restClient.ExecuteTaskAsync(request);
        }

        public async Task LogDosRequest(OutcomeViewModel model, DosViewModel dosViewModel)
        {
            var audit = model.ToAuditEntry();
            var auditedDosViewModel = Mapper.Map<AuditedDosRequest>(dosViewModel);
            audit.DosRequest = JsonConvert.SerializeObject(auditedDosViewModel);
            await Log(audit);
        }

        public async Task LogDosResponse(OutcomeViewModel model, DosCheckCapacitySummaryResult result)
        {
            var audit = model.ToAuditEntry();
            var auditedDosResponse = Mapper.Map<AuditedDosResponse>(result);
            audit.DosResponse = JsonConvert.SerializeObject(auditedDosResponse);
            await Log(audit);
        }

        public async Task LogSelectedService(OutcomeViewModel model)
        {
            await LogEventData(model, string.Format("User selected service '{0}' ({1})", model.SelectedService.Name, model.SelectedService.Id));
        }

        public async Task LogEventData(JourneyViewModel model, string eventData)
        {
            var audit = model.ToAuditEntry();
            audit.EventData = eventData;
            await Log(audit);
        }

        public async Task LogItkRequest(OutcomeViewModel model, ITKDispatchRequest itkRequest)
        {
            var audit = model.ToAuditEntry();
            var auditedItkRequest = Mapper.Map<AuditedItkRequest>(itkRequest);
            audit.ItkRequest = JsonConvert.SerializeObject(auditedItkRequest);
            await Log(audit);
        }

        public async Task LogItkResponse(OutcomeViewModel model, string response)
        {
            var audit = model.ToAuditEntry();
            var auditedItkResponse = Mapper.Map<AuditedItkResponse>(response);
            audit.ItkResponse = JsonConvert.SerializeObject(auditedItkResponse);
            await Log(audit);
        }

        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;
    }
}