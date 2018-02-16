using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Web.FromExternalServices;
using BusinessModels = NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.Service
{
    public class OnlineServiceTypeFilter : IOnlineServiceTypeFilter
    {
        public List<BusinessModels.DosService> Filter(List<BusinessModels.DosService> resultsToMap)
        {
            resultsToMap.RemoveAll(s => s.OnlineDOSServiceType == OnlineDOSServiceType.Unknown);
            return resultsToMap.ToList();
        }
    }

    public interface IOnlineServiceTypeFilter
    {
        List<BusinessModels.DosService> Filter(List<BusinessModels.DosService> resultsToMap);
    }
}
