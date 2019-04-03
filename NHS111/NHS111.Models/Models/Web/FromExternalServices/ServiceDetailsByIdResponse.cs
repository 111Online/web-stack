using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class ServiceDetailsByIdResponse
    {
        [JsonProperty(PropertyName = "services")]
        public ServiceDetails[] Services { get; set; }
    }
}
