using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosServicesByClinicalTermRequest
    {
        [JsonProperty(PropertyName = "caseId")]
        public string CaseId { get; set; }
        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }
        [JsonProperty(PropertyName = "searchDistance")]
        public string SearchDistance { get; set; }
        [JsonProperty(PropertyName = "gpPracticeId")]
        public string GpPracticeId { get; set; }
        [JsonProperty(PropertyName = "age")]
        public string Age { get; set; }
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "disposition")]
        public string Disposition { get; set; }
        [JsonProperty(PropertyName = "symptomGroupDiscriminatorCombos")]
        public string SymptomGroupDiscriminatorCombos { get; set; }
        [JsonProperty(PropertyName = "numberPerType")]
        public string NumberPerType { get; set; }
    }
}
