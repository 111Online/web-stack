using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosServicesByClinicalTermRequest
    {
        private string _caseId;
        private string _gpPracticeId;
        private string _numberPerType;
        private string _searchDistance;

        #region Optional Fields
        [JsonProperty(PropertyName = "caseId")]
        public string CaseId
        {
            get { return GetValueOrDefault(_caseId); }
            set { _caseId = value; }
        }
        [JsonProperty(PropertyName = "gpPracticeId")]
        public string GpPracticeId
        {
            get { return GetValueOrDefault(_gpPracticeId); }
            set { _gpPracticeId = value; }
        }
        [JsonProperty(PropertyName = "numberPerType")]
        public string NumberPerType
        {
            get { return GetValueOrDefault(_numberPerType); }
            set { _numberPerType = value; }
        }
        [JsonProperty(PropertyName = "searchDistance")]
        public string SearchDistance
        {
            get { return GetValueOrDefault(_searchDistance); }
            set { _searchDistance = value; }
        }
        #endregion

        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }
        [JsonProperty(PropertyName = "age")]
        public string Age { get; set; }
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }
        [JsonProperty(PropertyName = "disposition")]
        public string Disposition { get; set; }
        [JsonProperty(PropertyName = "symptomGroupDiscriminatorCombos")]
        public string SymptomGroupDiscriminatorCombos { get; set; }

        private static string GetValueOrDefault(string input)
        {
            return string.IsNullOrEmpty(input) ? "0" : input;
        }
    }
}

