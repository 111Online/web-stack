using System.Collections.Generic;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class GroupedDOSServices
    {
        public GroupedDOSServices(OnlineDOSServiceType serviceType, List<ServiceViewModel> services)
        {
            OnlineDOSServiceType = serviceType;
            Services = services;
        }
        public OnlineDOSServiceType OnlineDOSServiceType { get; private set; }
        public List<ServiceViewModel> Services { get; private set; }
    }
}