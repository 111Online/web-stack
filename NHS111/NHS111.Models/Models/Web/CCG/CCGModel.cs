using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.CCG
{
    public class CCGModel
    {
        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }

        [JsonProperty(PropertyName = "ccg")]
        public string CCG { get; set; }

        [JsonProperty(PropertyName = "app")]
        public string App { get; set; }
    }
}
