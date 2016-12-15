using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.ServiceAviliablility;
using NHS111.Models.Models.Web.DosRequests;
using NodaTime;

namespace NHS111.Business.DOS
{
    using Models.Models.Business;

    public class ServiceAvailablityManager : IServiceAvailabilityManager
    {
        private IConfiguration _configuration;
        public ServiceAvailablityManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceAvailability FindServiceAvailability(DosFilteredCase dosFilteredCase)
        {
            if (IsDentalDispoition(dosFilteredCase.Disposition)) return new DentalServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            if (IsPrimaryCareDispoition(dosFilteredCase.Disposition)) return new PrimaryCareServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            return new ServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
        }

        private IServiceAvailabilityProfile FindServiceAvailabilityProfile(int dxCode)
        {
           
            var primaryCareServiceTypeIdBlackist = ConvertPipeDeliminatedString(_configuration.FilteredPrimaryCareDosServiceIds);
            var dentalServiceTypeIdBlackist = ConvertPipeDeliminatedString(_configuration.FilteredDentalDosServiceIds);
            var clinicianCallbackServiceTypeIdBlackist = ConvertPipeDeliminatedString(_configuration.FilteredClinicianCallbackDosServiceIds);

            if (IsPrimaryCareDispoition(dxCode)) return new ServiceAvailabilityProfile(
                new PrimaryCareProfileHoursOfOperation(_configuration.WorkingDayPrimaryCareInHoursStartTime, _configuration.WorkingDayPrimaryCareInHoursShoulderEndTime, _configuration.WorkingDayPrimaryCareInHoursEndTime), primaryCareServiceTypeIdBlackist);

            if (IsDentalDispoition(dxCode)) return new ServiceAvailabilityProfile(
                new DentalProfileHoursOfOperation(_configuration.WorkingDayDentalInHoursStartTime, _configuration.WorkingDayDentalInHoursShoulderEndTime, _configuration.WorkingDayDentalInHoursEndTime), dentalServiceTypeIdBlackist);

            if (IsClinicianCallbackDispoition(dxCode)) return new ServiceAvailabilityProfile(
                new ClinicianCallbackProfileHoursOfOperation(), clinicianCallbackServiceTypeIdBlackist);

            return new ServiceAvailabilityProfile(new ProfileHoursOfOperation(new LocalTime(0, 0), new LocalTime(0, 0), new LocalTime(0, 0)), new List<int>());
        }

        private bool IsDentalDispoition(int dxCode)
        {
            return ConvertPipeDeliminatedString(_configuration.FilteredDentalDispositionCodes).Contains(dxCode);
        }

        private bool IsPrimaryCareDispoition(int dxCode)
        {
            return ConvertPipeDeliminatedString(_configuration.FilteredPrimaryCareDispositionCodes).Contains(dxCode);
        }

        private bool IsClinicianCallbackDispoition(int dxCode)
        {
            return ConvertPipeDeliminatedString(_configuration.FilteredClinicianCallbackDispositionCodes).Contains(dxCode);
        }


        private IEnumerable<int> ConvertPipeDeliminatedString(string pipedeliminatedString)
        {
            return pipedeliminatedString.Split('|').Select(c => Convert.ToInt32(c)).ToList();
        }
    }

    public class ClinicianCallbackProfileHoursOfOperation
        : IProfileHoursOfOperation {

        public ProfileServiceTimes GetServiceTime(DateTime date) {
            return ProfileServiceTimes.InHours;
        }

        public bool ContainsInHoursPeriod(DateTime startDateTime, DateTime endDateTime) {
            return true;
        }
    }
}
