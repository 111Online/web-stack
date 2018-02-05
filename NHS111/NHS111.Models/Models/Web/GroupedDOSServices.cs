using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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