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

        public Log4NetStorageProvider(ILog log) {
            if (log == null)
                throw new ArgumentNullException(string.Format("Cannot construct a {0} with a null {1}.", nameof(Log4NetStorageProvider), nameof(log)));

            _log = log;
        }

        public async Task Log(string request) {
            _log.Info(request);
        }

        public async Task Audit(AuditEntry audit) {
            var serialisedAudit = JsonConvert.SerializeObject(audit);
            _log.Audit(serialisedAudit);
        }

        private readonly ILog _log;
    }
}