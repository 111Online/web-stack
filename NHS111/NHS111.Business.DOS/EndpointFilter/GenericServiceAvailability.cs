using System;

namespace NHS111.Business.DOS.EndpointFilter
{
    public class GenericServiceAvailability : ServiceAvailability
    {
        public GenericServiceAvailability(IServiceAvailabilityProfile serviceAvailabilityProfile,
            DateTime dispositionDateTime, int dispostionTimeframe)
            : base(serviceAvailabilityProfile, dispositionDateTime, dispostionTimeframe)
        {

        }
    }
}
