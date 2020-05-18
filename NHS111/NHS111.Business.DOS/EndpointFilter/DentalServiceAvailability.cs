using NHS111.Models.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Business.DOS.EndpointFilter
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
            var itkservicestoRetain = GetSpecifiedOpenITKServices(resultsToFilter);
            var filteredResults = resultsToFilter.Where(
                s => !_serviceAvailabilityProfile.ServiceTypeIdBlacklist.Contains((int)s.ServiceType.Id)).ToList();
            filteredResults.AddRange(itkservicestoRetain.Where(NotDuplicateMessage(filteredResults)));
            return filteredResults;
        }
    }
}
