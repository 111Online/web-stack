using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class Pathway
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "pathwayNo")]
        public string PathwayNo { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "minimumAgeInclusive")]
        public int MinimumAgeInclusive { get; set; }

        [JsonProperty(PropertyName = "maximumAgeExclusive")]
        public int MaximumAgeExclusive { get; set; }

        [JsonProperty(PropertyName = "module")]
        public string Module { get; set; }

        [JsonProperty(PropertyName = "symptomGroup")]
        public string SymptomGroup { get; set; }

        [JsonProperty(PropertyName = "group")]
        public string Group { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "excludeKeywords")]
        public string ExcludeKeywords { get; set; }

        [JsonProperty(PropertyName = "startingPathway")]
        public bool StartingPathway { get; set; }

        [JsonProperty(PropertyName = "traumaType")]
        public string TraumaType { get; set; }

        [JsonProperty(PropertyName = "majorVersion")]
        public string MajorVersion { get; set; }

        [JsonProperty(PropertyName = "minorVersion")]
        public string MinorVersion { get; set; }

        [JsonProperty(PropertyName = "subRevision")]
        public string SubRevision { get; set; }


    }

}