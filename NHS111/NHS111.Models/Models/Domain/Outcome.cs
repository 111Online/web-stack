using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class Outcome
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "timeFrameText")]
        public string TimeFrameText { get; set; }

        [JsonProperty(PropertyName = "waitTimeText")]
        public string WaitTimeText { get; set; }

        [JsonProperty(PropertyName = "dispositionUrgencyText")]
        public string DispositionUrgencyText { get; set; }
    }
}