namespace NHS111.Logging.Api.StorageProviders {
    using System;
    using System.Threading.Tasks;
    using log4net;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Web.Logging;
    using Utils.Logging;

    public interface ILogStorageProvider {
        Task Log(string request);
        Task Audit(AuditEntry audit);
    }

    public class Log4NetStorageProvider
        : ILogStorageProvider {

        public async Task Log(string request) {
            Log4Net.Info(request);
        }

        public async Task Audit(AuditEntry audit) {
            var serialisedAudit = JsonConvert.SerializeObject(audit);
            Log4Net.Audit(serialisedAudit);
        }
    }
}