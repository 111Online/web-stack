using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.DOS.Configuration;
using NodaTime;

namespace NHS111.Business.DOS
{
    public class ServiceAvailablityProfileManager : IServiceAvailabilityProfileManager
    {
        private IConfiguration _configuration;
        public ServiceAvailablityProfileManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceAvailabilityProfile FindServiceAvailability(int dxCode)
        {
            var dentalDispoCodes = ConvertPipeDeliminatedString(_configuration.FilteredDentalDispositionCodes);
            var primaryCareDispoCodes = ConvertPipeDeliminatedString(_configuration.FilteredPrimaryCareDispositionCodes);
            var primaryCareServiceTypeIdBlackist = ConvertPipeDeliminatedString(_configuration.FilteredPrimaryCareDosServiceIds);
            var dentalServiceTypeIdBlackist = ConvertPipeDeliminatedString(_configuration.FilteredDentalDosServiceIds);

            if (primaryCareDispoCodes.Contains(dxCode)) return new ServiceAvailabilityProfile(
                new ProfileHoursOfOperation(_configuration.WorkingDayPrimaryCareInHoursStartTime, _configuration.WorkingDayPrimaryCareInHoursShoulderEndTime, _configuration.WorkingDayPrimaryCareInHoursEndTime), primaryCareServiceTypeIdBlackist);

            if (dentalDispoCodes.Contains(dxCode)) return new ServiceAvailabilityProfile(
                new ProfileHoursOfOperation(_configuration.WorkingDayPrimaryCareInHoursStartTime, _configuration.WorkingDayPrimaryCareInHoursShoulderEndTime, _configuration.WorkingDayPrimaryCareInHoursEndTime), dentalServiceTypeIdBlackist);

            return new ServiceAvailabilityProfile(new ProfileHoursOfOperation(new LocalTime(0, 0), new LocalTime(0, 0), new LocalTime(0, 0)), new List<int>());
        }

        private IEnumerable<int> ConvertPipeDeliminatedString(string pipedeliminatedString)
        {
            return pipedeliminatedString.Split('|').Select(c => Convert.ToInt32(c)).ToList();
        } 
    }
}
