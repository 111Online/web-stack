using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;
using System.Collections.Generic;

namespace NHS111.Models.Models.Business.Question
{
    public class FullPathwayJourney
    {
        [JsonProperty(PropertyName = "steps")]
        public IEnumerable<JourneyStep> JourneySteps { get; set; }
        [JsonProperty(PropertyName = "startingPathwayId")]
        public string StartingPathwayId { get; set; }
        [JsonProperty(PropertyName = "startingPathwayType")]
        public string StartingPathwayType { get; set; }
        [JsonProperty(PropertyName = "dispositionCode")]
        public string DispostionCode { get; set; }
        [JsonProperty(PropertyName = "state")]
        public IDictionary<string, string> State { get; set; }
    }
}
