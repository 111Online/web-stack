using NHS111.Models.Models.Business;
using NHS111.Models.Models.Business.Enums;
using System;
using System.Collections.Generic;

namespace NHS111.Business.DOS.EndpointFilter
{
    public class ServiceAvailabilityProfile : IServiceAvailabilityProfile
    {
        private readonly IProfileHoursOfOperation _profileHoursOfOperation;
        private readonly IEnumerable<int> _serviceTypeIdBlacklist;

        public ServiceAvailabilityProfile(IProfileHoursOfOperation profileHoursOfOperation, IEnumerable<int> serviceTypeIdBlacklist)
        {
            _profileHoursOfOperation = profileHoursOfOperation;
            _serviceTypeIdBlacklist = serviceTypeIdBlacklist;
        }

        public int ProfileId { get; set; }

        public string ProfileName { get; set; }

        public DispositionTimePeriod GetServiceAvailability(DateTime dispositionDateTime, int timeFrameMinutes)
        {
            var timeFrameDateTime = dispositionDateTime.AddMinutes(timeFrameMinutes);

            var dispositionServiceTime = _profileHoursOfOperation.GetServiceTime(dispositionDateTime);
            var timeFrameServiceTime = _profileHoursOfOperation.GetServiceTime(timeFrameDateTime);

            if (dispositionServiceTime == ProfileServiceTimes.InHoursShoulder && timeFrameServiceTime == ProfileServiceTimes.InHours)
                return DispositionTimePeriod.DispositionInShoulderTimeFrameInHours;

            if (dispositionServiceTime == ProfileServiceTimes.InHoursShoulder && timeFrameServiceTime == ProfileServiceTimes.InHoursShoulder)
                return DispositionTimePeriod.DispositionInShoulderTimeFrameInShoulder;

            if (dispositionServiceTime == ProfileServiceTimes.InHoursShoulder && timeFrameServiceTime == ProfileServiceTimes.OutOfHours)
                return DispositionTimePeriod.DispositionInShoulderTimeFrameOutOfHours;

            if (dispositionServiceTime == ProfileServiceTimes.InHours && timeFrameServiceTime == ProfileServiceTimes.InHours)
                return DispositionTimePeriod.DispositionAndTimeFrameInHours;

            if (dispositionServiceTime == ProfileServiceTimes.InHours && timeFrameServiceTime == ProfileServiceTimes.OutOfHours)
                return DispositionTimePeriod.DispositionInHoursTimeFrameOutOfHours;

            if (dispositionServiceTime == ProfileServiceTimes.OutOfHours && timeFrameServiceTime == ProfileServiceTimes.InHours)
                return DispositionTimePeriod.DispositionOutOfHoursTimeFrameInHours;

            if (dispositionServiceTime == ProfileServiceTimes.OutOfHours && timeFrameServiceTime == ProfileServiceTimes.InHoursShoulder)
                return DispositionTimePeriod.DispositionOutOfHoursTimeFrameInShoulder;

            var containsInHoursPeriod = _profileHoursOfOperation.ContainsInHoursPeriod(dispositionDateTime, timeFrameDateTime);
            if (dispositionServiceTime == ProfileServiceTimes.OutOfHours && timeFrameServiceTime == ProfileServiceTimes.OutOfHours && containsInHoursPeriod)
                return DispositionTimePeriod.DispositionAndTimeFrameOutOfHoursTraversesInHours;

            return DispositionTimePeriod.DispositionAndTimeFrameOutOfHours;
        }


        public IEnumerable<int> ServiceTypeIdBlacklist
        {
            get { return _serviceTypeIdBlacklist; }
        }
    }
}
