using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NHS111.Models.Mappers;

namespace NHS111.Models.Models.Domain
{
    using System;

    public class Answer
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }


        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }


        [JsonProperty(PropertyName = "titleHtmlStripped")]
        public string TitleHtmlStripped
        {
            get { return Regex.Replace(Title, "<[a-zA-Z/].*?>", String.Empty); }
        }


        [JsonProperty(PropertyName = "titleWithoutSpaces")]
        public string TitleWithoutSpaces {
            get
            {
                if (!String.IsNullOrEmpty(Id)) return Id;
                return TitleHtmlStripped != null ? TitleHtmlStripped.Replace(" ", string.Empty) : string.Empty;
            } }

        [JsonProperty(PropertyName = "symptomDiscriminator")]
        public string SymptomDiscriminator { get; set; }

        [JsonProperty(PropertyName = "supportingInfo")]
        public string SupportingInformation { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "excludeKeywords")]
        public string ExcludeKeywords { get; set; }

        [JsonProperty(PropertyName = "reportText")]
        public string ReportText { get; set; }

        [JsonProperty(PropertyName = "dispoDisplayText")]
        public string DispositionDisplayText { get; set; }

        [JsonIgnore]
        public string SupportingInformationHtml {
            get { return StaticTextToHtml.Convert(SupportingInformation); }
        }

        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }
        
        [JsonProperty(PropertyName = "isPositive")]
        public bool IsPositive { get; set; }

        [JsonProperty(PropertyName = "specifyText")]
        public string SpecifyText { get; set; }

        [JsonProperty(PropertyName = "snomedCodes")]
        public List<string> SnomedCodes { get; set; }

    }
}