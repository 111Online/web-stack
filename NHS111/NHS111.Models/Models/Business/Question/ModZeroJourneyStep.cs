using System;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Business.Question
{
    public class ModZeroJourneyStep : TableEntity
    {
        //PartitionKey == Gender
        //RowKey == Step Order

        [JsonProperty(PropertyName = "minimumAge")]
        public int MinimumAge { get; set; }

        [JsonProperty(PropertyName = "maximumAge")]
        public int MaximumAge { get; set; }

        [JsonProperty(PropertyName = "answerOrder")]
        public int AnswerOrder { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "pathwayId")]
        public string PathwayId { get; set; }

        [JsonProperty(PropertyName = "dispositionId")]
        public string DispositionId { get; set; }
    }
}
