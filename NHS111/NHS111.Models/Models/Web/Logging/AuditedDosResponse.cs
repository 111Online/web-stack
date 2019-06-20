using NHS111.Models.Models.Web.FromExternalServices;
using System.Linq;

namespace NHS111.Models.Models.Web.Logging
{
    public class AuditedDosResponse : DosCheckCapacitySummaryResult
    {
        public bool DosResultsContainItkOfferring { get { return !ResultListEmpty && Success.Services.Any(s => s.OnlineDOSServiceType.IsReferral); }  }
    }
}
