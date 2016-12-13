using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Business.DOS.ServiceAviliablility
{
    public class DentalServiceAvailability : ServiceAvailability
    {
        private IServiceAvailabilityProfile _serviceAvailabilityProfile;
        public DentalServiceAvailability(IServiceAvailabilityProfile serviceAvailabilityProfile,
            DateTime dispositionDateTime, int dispostionTimeframe)
            : base(serviceAvailabilityProfile, dispositionDateTime, dispostionTimeframe)
        {
            _serviceAvailabilityProfile = serviceAvailabilityProfile;
        }

        public override List<DosService> Filter(List<DosService> resultsToFilter)
        {
            return resultsToFilter.Where(
                s => !_serviceAvailabilityProfile.ServiceTypeIdBlacklist.Contains((int) s.ServiceType.Id)).ToList();
        }
    }
}
