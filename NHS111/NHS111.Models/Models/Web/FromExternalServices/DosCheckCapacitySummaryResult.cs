using System;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class DosCheckCapacitySummaryResult
    {
        [JsonProperty(PropertyName = "success")]
        public SuccessObject<ServiceViewModel> Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ErrorObject Error { get; set; }

        public bool ResultListEmpty {
            get { return Error != null || (Success != null && Success.Services.Count <= 0); }
        }

        public bool ServicesUnavailable { get; set; }
    }
}
