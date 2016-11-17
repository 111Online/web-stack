using System;
using NHS111.Models.Models.Business.Enums;

namespace NHS111.Models.Models.Business
{
    public interface IServiceAvailabilityProfile
    {
        int ProfileId { get; set; }

        string ProfileName { get; set; }

        ProfileHoursOfOperation OperatingHours { get; set; }

        DispositionTimePeriod GetServiceAvailability(DateTime dispositionDateTime, int timeFrameMinutes);
    }
}
