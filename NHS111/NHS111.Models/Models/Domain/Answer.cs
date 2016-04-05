using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    using System;

    public class Answer
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "titleWithoutSpaces")]
        public string TitleWithoutSpaces { get { return Title.Replace(" ", String.Empty); } }

        [JsonProperty(PropertyName = "symptomDiscriminator")]
        public string SymptomDiscriminator { get; set; }

        [JsonProperty(PropertyName = "supportingInfo")]
        public string SupportingInformation { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }
        
    }
}