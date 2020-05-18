using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class CheckCapacitySummaryResponse
    {
        public string TransactionId { get; set; }
        public string RequestedAtDateTime { get; set; }
        public string SearchDateTime { get; set; }

        [JsonProperty(PropertyName = "CheckCapacitySummaryResult")]
        public DosService[] CheckCapacitySummaryResult { get; set; }
    }
}
