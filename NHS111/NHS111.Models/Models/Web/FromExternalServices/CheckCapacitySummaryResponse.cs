using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class CheckCapacitySummaryResponse
    {
        public string TransactionId { get; set; }
        public string RequestedAtDateTime { get; set; }
        public string SearchDateTime { get; set; }

        [JsonProperty(PropertyName = "CheckCapacitySummaryResult")]
        public CheckCapacitySummaryResult[] CheckCapacitySummaryResult { get; set; }
    }
}
