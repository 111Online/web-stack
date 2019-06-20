using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Business.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using DosService = NHS111.Models.Models.Business.DosService;

namespace NHS111.Business.DOS.EndpointFilter
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


        public virtual List<Models.Models.Business.DosService> Filter(List<Models.Models.Business.DosService> resultsToFilter)
        {
            var itkservicestoRetain = GetSpecifiedOpenITKServices(resultsToFilter);
            var fileteredServices = !this.IsOutOfHours
                ? resultsToFilter.Where(
                    s => !_serviceAvailabilityProfile.ServiceTypeIdBlacklist.Contains((int)s.ServiceType.Id)).ToList()
                : resultsToFilter;
            fileteredServices.AddRange(itkservicestoRetain.Where(NotDuplicateMessage(fileteredServices))); 
            return fileteredServices;
        }

        protected  Func<DosService, bool> NotDuplicateMessage(List<DosService> fileteredServices)
        {
            return s => !fileteredServices.Any(f => f.Id == s.Id);
        }


        protected List<Models.Models.Business.DosService> GetSpecifiedOpenITKServices(List<Models.Models.Business.DosService> dosServices)
        {
            return dosServices.Where(r => (r.OnlineDOSServiceType.IsReferral) && r.IsOpenForSpecifiedTimes).ToList(); ;
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
