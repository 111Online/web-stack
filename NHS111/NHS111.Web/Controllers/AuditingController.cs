
using NHS111.Utils.Attributes;

namespace NHS111.Web.Controllers {
    using System.Net;
    using System.Web.Mvc;
    using Models.Models.Web.Logging;
    using NHS111.Web.Presentation.Logging;

    [LogHandleErrorForMVC]
    public class AuditingController
        : Controller {
        
        public AuditingController(IAuditLogger auditLogger) {
            _auditLogger = auditLogger;
        }

        [HttpPost]
        public ActionResult Log(AuditViewModel audit) {
            _auditLogger.Log(audit.ToAuditEntry());

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private readonly IAuditLogger _auditLogger;
    }
}