
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
            : this(new Log4NetStorageProvider()) {
            
        }

        public LogsController(ILogStorageProvider logger) {
            if (logger == null)
                throw new ArgumentNullException("logger","Cannot construct a LogsController with a null logger");

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
                return BadRequest("Cannot log a null AuditEntry");

            if (audit.SessionId == Guid.Empty)
                return BadRequest("AuditEntry.SessionId cannot be empty.");

            await _logger.Audit(audit);

            return Ok();
        }

        private readonly ILogStorageProvider _logger;
    }
}