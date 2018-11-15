using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Azure
{
    public class ModZeroJourneyStep : TableEntity
    {
        //PartitionKey == {Gender}{AgeGroup}{MaxAge}{Type}-{Party}
        //RowKey == Step Order

        [JsonProperty(PropertyName = "questionId")]
        public string QuestionId { get; set; }
        [JsonProperty(PropertyName = "answerOrder")]
        public int AnswerOrder { get; set; }
    }
}
