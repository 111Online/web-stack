
namespace NHS111.Web.Presentation.Logging {
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Configuration;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Web.Logging;
    using Utils.Helpers;

    public interface IAuditLogger {
        Task Log(AuditEntry auditEntry);
    }

    public class AuditLogger
        : IAuditLogger {
        
        public AuditLogger(IRestfulHelper restfulHelper, IConfiguration configuration) {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task Log(AuditEntry auditEntry) {
            var url = _configuration.LoggingServiceUrl;
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
            {
                Content = new StringContent(JsonConvert.SerializeObject(auditEntry))
            };
            await _restfulHelper.PostAsync(url, httpRequestMessage);
        }

        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
    }
}