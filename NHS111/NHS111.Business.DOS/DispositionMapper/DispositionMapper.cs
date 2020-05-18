using NHS111.Business.DOS.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Business.DOS.DispositionMapper
{
    public class DispositionMapper : IDispositionMapper
    {
        private readonly IConfiguration _configuration;

        public DispositionMapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsDentalDisposition(int dxCode)
        {
            return ConvertDispositionString(_configuration.FilteredDentalDispositionCodes).Contains(dxCode);
        }

        public bool IsPrimaryCareDisposition(int dxCode)
        {
            return ConvertDispositionString(_configuration.FilteredPrimaryCareDispositionCodes).Contains(dxCode);
        }

        public bool IsClinicianCallbackDisposition(int dxCode)
        {
            return ConvertDispositionString(_configuration.FilteredClinicianCallbackDispositionCodes).Contains(dxCode);
        }

        public bool IsRepeatPrescriptionDisposition(int dxCode)
        {
            return ConvertDispositionString(_configuration.FilteredRepeatPrescriptionDispositionCodes).Contains(dxCode);
        }

        public bool IsGenericDisposition(int dxCode)
        {
            return ConvertDispositionString(_configuration.FilteredGenericDispositionCodes).Contains(dxCode);
        }

        public IEnumerable<int> ConvertDispositionString(string pipedeliminatedString)
        {
            if (String.IsNullOrWhiteSpace(pipedeliminatedString)) return new List<int>();
            return pipedeliminatedString.Split('|').Select(c => Convert.ToInt32(c)).ToList();
        }
    }

    public interface IDispositionMapper
    {
        bool IsDentalDisposition(int dxCode);
        bool IsPrimaryCareDisposition(int dxCode);
        bool IsClinicianCallbackDisposition(int dxCode);
        bool IsRepeatPrescriptionDisposition(int dxCode);
        bool IsGenericDisposition(int dxCode);
        IEnumerable<int> ConvertDispositionString(string pipedeliminatedString);
    }
}
