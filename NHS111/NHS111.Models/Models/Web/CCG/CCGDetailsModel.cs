using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.CCG
{
    public class CCGDetailsModel :CCGModel
    {
        [JsonProperty(PropertyName = "stpName")]
        public string StpName { get; set; }

        [JsonProperty(PropertyName = "referralServiceIdWhitelist")]
        public ServiceListModel ReferralServiceIdWhitelist { get; set; }

        [JsonProperty(PropertyName = "pharmacyReferralServiceIdWhitelist")]
        public ServiceListModel PharmacyReferralServiceIdWhitelist { get; set; }

        public bool PharmacyServicesAvailable
        {
            get
            {
                return (PharmacyReferralServiceIdWhitelist != null && PharmacyReferralServiceIdWhitelist.Count > 0);
            }
        }
    }
}
