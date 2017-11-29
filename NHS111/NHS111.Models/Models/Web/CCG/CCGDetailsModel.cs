using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.CCG
{
    public class CCGDetailsModel :CCGModel
    {
        [JsonProperty(PropertyName = "stpName")]
        public string StpName { get; set; }

        [JsonProperty(PropertyName = "serviceIdWhitelist")]
        public ServiceListModel ServiceIdWhitelist { get; set; }

        [JsonProperty(PropertyName = "itkServiceIdWhitelist")]
        public ServiceListModel ItkServiceIdWhitelist { get; set; }
    }
}
