using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.DosRequests;
using NodaTime;
using NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.EndpointFilter
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
            if (IsDentalDispoition(dosFilteredCase.Disposition)) return new DentalServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            if (IsPrimaryCareDispoition(dosFilteredCase.Disposition)) return new PrimaryCareServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            if (IsRepeatPrescriptionDisposition(dosFilteredCase.Disposition)) return new RepeatPrescriptionServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            if (IsGenericDisposition(dosFilteredCase.Disposition)) return new GenericServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            return new ServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
        }

        private IServiceAvailabilityProfile FindServiceAvailabilityProfile(int dxCode)
        {
           
            var primaryCareServiceTypeIdBlackist = ConvertPipeDeliminatedString(_configuration.FilteredPrimaryCareDosServiceIds);
            var dentalServiceTypeIdBlackist = ConvertPipeDeliminatedString(_configuration.FilteredDentalDosServiceIds);
            var clinicianCallbackServiceTypeIdBlackist = ConvertPipeDeliminatedString(_configuration.FilteredClinicianCallbackDosServiceIds);
            var repeatPrescriptionServiceTypeIdBlackist = ConvertPipeDeliminatedString(_configuration.FilteredRepeatPrescriptionDosServiceIds);
            var genericServiceTypeIdBlackList = ConvertPipeDeliminatedString(_configuration.FilteredGenericDosServiceIds);

            if (IsPrimaryCareDispoition(dxCode)) return new ServiceAvailabilityProfile(
                new PrimaryCareProfileHoursOfOperation(_configuration.WorkingDayPrimaryCareInHoursStartTime, _configuration.WorkingDayPrimaryCareInHoursShoulderEndTime, _configuration.WorkingDayPrimaryCareInHoursEndTime, _configuration), primaryCareServiceTypeIdBlackist);

            if (IsDentalDispoition(dxCode)) return new ServiceAvailabilityProfile(
                new DentalProfileHoursOfOperation(_configuration.WorkingDayDentalInHoursStartTime, _configuration.WorkingDayDentalInHoursShoulderEndTime, _configuration.WorkingDayDentalInHoursEndTime, _configuration), dentalServiceTypeIdBlackist);

            if (IsClinicianCallbackDispoition(dxCode)) return new ServiceAvailabilityProfile(
                new ClinicianCallbackProfileHoursOfOperation(), clinicianCallbackServiceTypeIdBlackist);

            if (IsRepeatPrescriptionDisposition(dxCode)) return new ServiceAvailabilityProfile(
                new RepeatPrescriptionProfileHoursOfOperation(), repeatPrescriptionServiceTypeIdBlackist);

            if (IsGenericDisposition(dxCode)) return new ServiceAvailabilityProfile(
                new GenericProfileHoursOfOperation(_configuration.WorkingDayGenericInHoursStartTime, _configuration.WorkingDayGenericInHoursShoulderEndTime, _configuration.WorkingDayGenericInHoursEndTime, _configuration), genericServiceTypeIdBlackList);

            return new ServiceAvailabilityProfile(new ProfileHoursOfOperation(new LocalTime(0, 0), new LocalTime(0, 0), new LocalTime(0, 0), _configuration), new List<int>());
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

        private bool IsRepeatPrescriptionDisposition(int dxCode)
        {
            return ConvertPipeDeliminatedString(_configuration.FilteredRepeatPrescriptionDispositionCodes).Contains(dxCode);
        }

        private bool IsGenericDisposition(int dxCode)
        {
            return ConvertPipeDeliminatedString(_configuration.FilteredGenericDispositionCodes).Contains(dxCode);
        }

        private IEnumerable<int> ConvertPipeDeliminatedString(string pipedeliminatedString)
        {
            if(String.IsNullOrWhiteSpace(pipedeliminatedString)) return new List<int>();
            return pipedeliminatedString.Split('|').Select(c => Convert.ToInt32(c)).ToList();
        }
    }

    public class RepeatPrescriptionProfileHoursOfOperation : IProfileHoursOfOperation
    {
        public ProfileServiceTimes GetServiceTime(DateTime date)
        {
            return ProfileServiceTimes.InHours;
        }

        public bool ContainsInHoursPeriod(DateTime startDateTime, DateTime endDateTime)
        {
            return true;
        }
    }

    public class ClinicianCallbackProfileHoursOfOperation
        : IProfileHoursOfOperation
    {

        public ProfileServiceTimes GetServiceTime(DateTime date)
        {
            return ProfileServiceTimes.InHours;
        }

        public bool ContainsInHoursPeriod(DateTime startDateTime, DateTime endDateTime)
        {
            return true;
        }
    }
}
