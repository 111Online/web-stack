using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.DispositionMapper;
using NHS111.Models.Models.Web.DosRequests;
using NodaTime;
using NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.EndpointFilter
{
    public class ServiceAvailablityManager : IServiceAvailabilityManager
    {
        private IConfiguration _configuration;
        private IDispositionMapper _dispositionMapper;

        public ServiceAvailablityManager(IConfiguration configuration, IDispositionMapper dispositionMapper)
        {
            _configuration = configuration;
            _dispositionMapper = dispositionMapper;
        }

        public IServiceAvailability FindServiceAvailability(DosFilteredCase dosFilteredCase)
        {
            if (_dispositionMapper.IsDentalDisposition(dosFilteredCase.Disposition)) return new DentalServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            if (_dispositionMapper.IsPrimaryCareDisposition(dosFilteredCase.Disposition)) return new PrimaryCareServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            if (_dispositionMapper.IsRepeatPrescriptionDisposition(dosFilteredCase.Disposition)) return new RepeatPrescriptionServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            if (_dispositionMapper.IsGenericDisposition(dosFilteredCase.Disposition)) return new GenericServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
            return new ServiceAvailability(FindServiceAvailabilityProfile(dosFilteredCase.Disposition), dosFilteredCase.DispositionTime, dosFilteredCase.DispositionTimeFrameMinutes);
        }

        private IServiceAvailabilityProfile FindServiceAvailabilityProfile(int dxCode)
        {
            if (_dispositionMapper.IsPrimaryCareDisposition(dxCode))
            {
                var primaryCareServiceTypeIdBlackList = _dispositionMapper.ConvertDispositionString(_configuration.FilteredPrimaryCareDosServiceIds);
                
                return new ServiceAvailabilityProfile(
                    new PrimaryCareProfileHoursOfOperation(_configuration.WorkingDayPrimaryCareInHoursStartTime,
                        _configuration.WorkingDayPrimaryCareInHoursShoulderEndTime,
                        _configuration.WorkingDayPrimaryCareInHoursEndTime, _configuration),
                    primaryCareServiceTypeIdBlackList);
            }

            if (_dispositionMapper.IsDentalDisposition(dxCode))
            {
                var dentalServiceTypeIdBlackList =
                    _dispositionMapper.ConvertDispositionString(_configuration.FilteredDentalDosServiceIds);

                return new ServiceAvailabilityProfile(
                    new DentalProfileHoursOfOperation(_configuration.WorkingDayDentalInHoursStartTime,
                        _configuration.WorkingDayDentalInHoursShoulderEndTime,
                        _configuration.WorkingDayDentalInHoursEndTime, _configuration), dentalServiceTypeIdBlackList);
            }

            if (_dispositionMapper.IsClinicianCallbackDisposition(dxCode))
            {
                var clinicianCallbackServiceTypeIdBlackList =
                    _dispositionMapper.ConvertDispositionString(_configuration.FilteredClinicianCallbackDosServiceIds);

                return new ServiceAvailabilityProfile(
                    new ClinicianCallbackProfileHoursOfOperation(), clinicianCallbackServiceTypeIdBlackList);
            }

            if (_dispositionMapper.IsRepeatPrescriptionDisposition(dxCode))
            {
                var repeatPrescriptionServiceTypeIdBlackList =
                    _dispositionMapper.ConvertDispositionString(_configuration.FilteredRepeatPrescriptionDosServiceIds);

                return new ServiceAvailabilityProfile(
                    new RepeatPrescriptionProfileHoursOfOperation(), repeatPrescriptionServiceTypeIdBlackList);

            }

            if (_dispositionMapper.IsGenericDisposition(dxCode))
            {
                var genericServiceTypeIdBlackList =
                    _dispositionMapper.ConvertDispositionString(_configuration.FilteredGenericDosServiceIds);

                return new ServiceAvailabilityProfile(
                new GenericProfileHoursOfOperation(_configuration.WorkingDayGenericInHoursStartTime, _configuration.WorkingDayGenericInHoursShoulderEndTime, _configuration.WorkingDayGenericInHoursEndTime, _configuration), genericServiceTypeIdBlackList);

            }

            return new ServiceAvailabilityProfile(new ProfileHoursOfOperation(new LocalTime(0, 0), new LocalTime(0, 0), new LocalTime(0, 0), _configuration), new List<int>());
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
