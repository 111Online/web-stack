using System;
using NHS111.Models.Models.Business.Enums;

namespace NHS111.Models.Models.Business
{
    public abstract class ServiceAvailabilityProfile : IServiceAvailabilityProfile
    {
        protected ServiceAvailabilityProfile()
        {

        }

        public virtual int ProfileId { get; set; }

        public virtual string ProfileName { get; set; }

        public virtual ProfileHoursOfOperation OperatingHours { get; set; }

        public virtual DispositionTimePeriod GetServiceAvailability(DateTime dispositionDateTime, int timeFrameMinutes)
        {
            throw new NotImplementedException();
        }
    }
}
