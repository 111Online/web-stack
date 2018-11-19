using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Business.Question
{
    public class PathwayJourney
    {
        [JsonProperty(PropertyName = "pathwayId")]
        public string PathwayId { get; set; }
        [JsonProperty(PropertyName = "dispositionId")]
        public string DispositionId { get; set; }
        [JsonProperty(PropertyName = "steps")]
        public IEnumerable<JourneyStep> Steps { get; set; }
        [JsonProperty(PropertyName = "state")]
        public IDictionary<string, string> State { get; set; }
    }
}
