using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Azure
{
    public class ModZeroJourney : TableEntity
    {
        //PartitionKey == ModZeroJourney
        //RowKey == {Gender}{AgeGroup}{MaxAge}{Type}-{Party}

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "minimumAge")]
        public int MinimumAge { get; set; }
        [JsonProperty(PropertyName = "maximumAge")]
        public int MaximumAge { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "pathwayId")]
        public string PathwayId { get; set; }
        [JsonProperty(PropertyName = "dispositionId")]
        public string DispositionId { get; set; }
        [JsonProperty(PropertyName = "party")]
        public int Party { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}
