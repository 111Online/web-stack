using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.DosRequests;
using NodaTime;

namespace NHS111.Business.DOS
{
    public class ServiceAvailablityManager : IServiceAvailabilityManager
    {
        private IConfiguration _configuration;
        public ServiceAvailablityManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceAvailability FindServiceAvailability(DosFilteredCase dosFilteredCase)
        {
            return new ServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
        }

        private IServiceAvailabilityProfile FindServiceAvailabilityProfile(int dxCode)
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
