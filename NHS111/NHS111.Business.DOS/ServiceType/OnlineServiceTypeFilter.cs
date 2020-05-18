using NHS111.Models.Models.Web.FromExternalServices;
using System.Collections.Generic;
using System.Linq;
using BusinessModels = NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.Service
{
    public class OnlineServiceTypeFilter : IOnlineServiceTypeFilter
    {
        public List<BusinessModels.DosService> FilterUnknownTypes(List<BusinessModels.DosService> resultsToMap)
        {
            resultsToMap.RemoveAll(s => s.OnlineDOSServiceType == OnlineDOSServiceType.Unknown);
            return resultsToMap.ToList();
        }

        public List<BusinessModels.DosService> FilterClosedCallbackServices(List<BusinessModels.DosService> resultsToMap)
        {
            resultsToMap.RemoveAll(s => s.OnlineDOSServiceType.IsReferral && !s.IsOpen);
            return resultsToMap.ToList();
        }
    }

    public interface IOnlineServiceTypeFilter
    {
        List<BusinessModels.DosService> FilterUnknownTypes(List<BusinessModels.DosService> resultsToMap);
        List<BusinessModels.DosService> FilterClosedCallbackServices(List<BusinessModels.DosService> resultsToMap);
    }
}
