using System;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Business.Enums;

namespace NHS111.Business.DOS
{
    public class ServiceAvailabilityProfile : IServiceAvailabilityProfile
    {
        public ServiceAvailabilityProfile()
        {

        }

        public int ProfileId { get; set; }

        public string ProfileName { get; set; }

        public ProfileHoursOfOperation OperatingHours { get; set; }

        public DispositionTimePeriod GetServiceAvailability(DateTime dispositionDateTime, int timeFrameMinutes)
        {
            return DispositionTimePeriod.DispositionAndTimeFrameInHours;
        }
    }
}
