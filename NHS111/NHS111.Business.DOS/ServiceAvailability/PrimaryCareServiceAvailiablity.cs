using System;

namespace NHS111.Business.DOS.ServiceAvailability
{
    public class PrimaryCareServiceAvailability : ServiceAvailability
    {
        public PrimaryCareServiceAvailability(IServiceAvailabilityProfile serviceAvailabilityProfile,
            DateTime dispositionDateTime, int dispostionTimeframe)
            : base(serviceAvailabilityProfile, dispositionDateTime, dispostionTimeframe)
        {
            
        }
    }
}
