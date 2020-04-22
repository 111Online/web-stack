using AutoMapper;
using NHS111.Utils.Attributes;

namespace NHS111.Web.Controllers
{
    using Models.Models.Web.Logging;
    using NHS111.Web.Presentation.Logging;
    using System.Net;
    using System.Web.Mvc;

    [LogHandleErrorForMVC]
    public class AuditingController
        : Controller
    {

        public AuditingController(IAuditLogger auditLogger)
        {
            _auditLogger = auditLogger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Log(PublicAuditViewModel audit)
        {
            var model = Mapper.Map<PublicAuditViewModel, AuditViewModel>(audit);
            _auditLogger.Log(model.ToAuditEntry());

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private readonly IAuditLogger _auditLogger;
    }
}