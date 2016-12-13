using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Business.Enums;
using NHS111.Models.Models.Web.DosRequests;

namespace NHS111.Business.DOS.ServiceAviliablility
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

        public virtual List<Models.Models.Web.FromExternalServices.DosService> Filter(List<Models.Models.Web.FromExternalServices.DosService> resultsToFilter)
        {
            return !this.IsOutOfHours
                ? resultsToFilter.Where(
                    s => !_serviceAvailabilityProfile.ServiceTypeIdBlacklist.Contains((int) s.ServiceType.Id)).ToList()
                : resultsToFilter;
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
