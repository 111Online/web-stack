using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Business
{
    public class SelectedAnswerState
    {
        [JsonProperty(PropertyName = "questionnaireId")]
        public string QuestionnaireId { get; set; }

        [JsonProperty(PropertyName = "answer")]
        public Answer  SelectedAnswer { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }
}
