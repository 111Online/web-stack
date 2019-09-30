using System;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.Enums;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class JourneyStep
    {
        [JsonProperty(PropertyName = "answer")]
        public Answer Answer { get; set; }

        [JsonProperty(PropertyName = "questionTitle")]
        public string QuestionTitle { get; set; }

        [JsonProperty(PropertyName = "questionNo")]
        public string QuestionNo { get; set; }

        [JsonProperty(PropertyName = "questionId")]
        public string QuestionId { get; set; }

        [JsonProperty(PropertyName = "jtbs")]
        public bool IsJustToBeSafe { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "stepType")]
        public NodeType NodeType { get; set; }

        public string NodeLabel
        {
            get
            {
                if (QuestionId.StartsWith("PX") && string.IsNullOrEmpty(QuestionNo)) return "Page";

                return string.IsNullOrEmpty(QuestionNo) || QuestionNo.StartsWith("TX") ? "Question" : "CareAdvice";
            }
        }

        public JourneyStep()
        {
            IsJustToBeSafe = false;
        }
    }
}