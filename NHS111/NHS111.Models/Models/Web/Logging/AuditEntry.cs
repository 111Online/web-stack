using System;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.Logging
{
    public class AuditEntry : LogEntry
    {
        [JsonProperty(PropertyName = "sessionId")]
        public Guid SessionId { get; set; }

        [JsonProperty(PropertyName = "pathwayId")]
        public string PathwayId { get; set; }

        [JsonProperty(PropertyName = "pathwayTitle")]
        public string PathwayTitle { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "journey")]
        public string Journey { get; set; }

        [JsonProperty(PropertyName = "answerTitle")]
        public string AnswerTitle { get; set; }

        [JsonProperty(PropertyName = "answerOrder")]
        public int AnswerOrder { get; set; }

        [JsonProperty(PropertyName = "questionTitle")]
        public string QuestionTitle { get; set; }

        [JsonProperty(PropertyName = "questionNo")]
        public string QuestionNo { get; set; }

        [JsonProperty(PropertyName = "questionId")]
        public string QuestionId { get; set; }
    }
}
