using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class ErrorObject
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
