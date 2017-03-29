using NHS111.Models.Models.Web.ITK;

namespace NHS111.Models.Models.Web.Logging
{
    public class AuditedItkRequest
    {
        public ServiceDetails ServiceDetails { get; set; }
        public CaseDetails CaseDetails { get; set; }
    }
}
