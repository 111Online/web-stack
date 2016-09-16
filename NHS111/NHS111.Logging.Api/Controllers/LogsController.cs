
namespace NHS111.Logging.Api.Controllers {
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using log4net;
    using NHS111.Models.Models.Web.Logging;
    using StorageProviders;

    [RoutePrefix("logs")]
    public class LogsController
        : ApiController {

        public LogsController()
            : this(new Log4NetStorageProvider(LogManager.GetLogger(typeof(Log4NetStorageProvider)))) {
            
        }

        public LogsController(ILogStorageProvider logger) {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger), string.Format("Cannot construct a {0} with a null {1}", nameof(LogsController), nameof(logger)));

            _logger = logger;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Log(string request) {

            await _logger.Log(request);
            return Ok();
        }

        [HttpPost, Route("audit")]
        public async Task<IHttpActionResult> Audit(AuditEntry audit) {

            if (audit == null)
                return BadRequest(string.Format("Cannot log a null {0}", nameof(AuditEntry)));

            if (audit.SessionId == Guid.Empty)
                return BadRequest(string.Format("{0} cannot be empty.", nameof(AuditEntry.SessionId)));

            await _logger.Audit(audit);

            return Ok();
        }

        private readonly ILogStorageProvider _logger;
    }
}