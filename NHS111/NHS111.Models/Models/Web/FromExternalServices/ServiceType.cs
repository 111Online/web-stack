using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class ServiceType
    {
        [JsonProperty(PropertyName = "idField")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "nameField")]
        public string Name { get; set; }
    }
}
