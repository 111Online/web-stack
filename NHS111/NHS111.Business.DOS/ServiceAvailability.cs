using System;
using NHS111.Models.Models.Business.Enums;

namespace NHS111.Business.DOS
{
    public class ServiceAvailability : IServiceAvailability
    {
        private readonly IServiceAvailabilityProfile _serviceAvailabilityProfile;

        private readonly DateTime _dispositionDateTime;

        private readonly int _timeFrameMinutes;

        public ServiceAvailability(IServiceAvailabilityProfile serviceAvailabilityProfile, DateTime dispositionDateTime, int timeFrameDays, int timeFrameHours, int timeFrameMinutes) : this(serviceAvailabilityProfile, dispositionDateTime, ConvertDaysHoursToMinutes(timeFrameDays, timeFrameHours, timeFrameMinutes))
        {
        }

        public ServiceAvailability(IServiceAvailabilityProfile serviceAvailabilityProfile, DateTime dispositionDateTime, int timeFrameHours, int timeFrameMinutes) : this(serviceAvailabilityProfile, dispositionDateTime, ConvertDaysHoursToMinutes(0, timeFrameHours, timeFrameMinutes))
        {
        }

        public ServiceAvailability(IServiceAvailabilityProfile serviceAvailabilityProfile, DateTime dispositionDateTime, int timeFrameMinutes)
        {
            _serviceAvailabilityProfile = serviceAvailabilityProfile;
            _dispositionDateTime = dispositionDateTime;
            _timeFrameMinutes = timeFrameMinutes;
        }

        public bool IsOutOfHours
        {
            get
            {
                var dispositionTimePeriod = _serviceAvailabilityProfile.GetServiceAvailability(_dispositionDateTime, _timeFrameMinutes);
                return dispositionTimePeriod == DispositionTimePeriod.DispositionAndTimeFrameOutOfHours || dispositionTimePeriod == DispositionTimePeriod.DispositionOutOfHoursTimeFrameInShoulder;
            }
        }

        private static int ConvertDaysHoursToMinutes(int days, int hours, int minutes)
        {
            var daysToMins = days*24*60;
            var hoursToMins = hours*60;
            return daysToMins + hoursToMins + minutes;
        }
    }
}
