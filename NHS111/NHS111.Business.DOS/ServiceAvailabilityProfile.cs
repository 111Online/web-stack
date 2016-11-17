using System;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Business.Enums;

namespace NHS111.Business.DOS
{
    public class ServiceAvailabilityProfile : IServiceAvailabilityProfile
    {
        private DateTime _inHoursStart = DateTime.Now.Date + new TimeSpan(8, 00, 00);
        private DateTime _outOfHoursStart = DateTime.Now.Date + new TimeSpan(20, 00, 00);

        public ServiceAvailabilityProfile()
        {

        }

        public int ProfileId { get; set; }

        public string ProfileName { get; set; }

        public ProfileHoursOfOperation OperatingHours { get; set; }

        public DispositionTimePeriod GetServiceAvailability(DateTime dispositionDateTime, int timeFrameMinutes)
        {
            var timeFrame = dispositionDateTime.AddMinutes(timeFrameMinutes);

            if (IsInHours(dispositionDateTime.Hour) && IsInHours(timeFrame.Hour))
                return DispositionTimePeriod.DispositionAndTimeFrameInHours;

            if (IsInHours(dispositionDateTime.Hour) && IsOutOfHours(timeFrame.Hour))
                return DispositionTimePeriod.DispositionInHoursTimeFrameOutOfHours;

            if (IsOutOfHours(dispositionDateTime.Hour) && IsInHours(timeFrame.Hour))
                return DispositionTimePeriod.DispositionOutOfHoursTimeFrameInHours;

            if(IsOutOfHours(dispositionDateTime.Hour) && IsOutOfHours(timeFrame.Hour) && (dispositionDateTime.Hour < _inHoursStart.Hour && timeFrame.Hour > _outOfHoursStart.Hour))
                return DispositionTimePeriod.DispositionAndTimeFrameOutOfHoursTraversesInHours;

            return DispositionTimePeriod.DispositionAndTimeFrameOutOfHours;
        }

        private bool IsInHours(int hour)
        {
            return hour >= _inHoursStart.Hour && hour < _outOfHoursStart.Hour;
        }

        private bool IsOutOfHours(int hour)
        {
            return hour >= _outOfHoursStart.Hour;
        }
    }
}
