using System;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    using System.Linq;

    public class DosCheckCapacitySummaryResult
    {
        [JsonProperty(PropertyName = "success")]
        public SuccessObject<ServiceViewModel> Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ErrorObject Error { get; set; }

        public bool ResultListEmpty {
            get {
                return Error != null || Success == null || (Success.Services == null || Success.Services.Count <= 0);
            }
        }

        public bool ServicesUnavailable { get; set; }

        public bool HasITKServices {  get { return !ResultListEmpty && Success.Services.Any(s => s.OnlineDOSServiceType.IsReferral); } }

        public bool IsValidationRequery { get; set; }

        public bool ContainsService(DosService selectedService) {
            return !ResultListEmpty && selectedService != null && (Success.Services.Exists(s => s.Id == selectedService.Id) || Success.Services.Exists(s => s.PublicName == "encounterReportPath"));
        }
    }
}
