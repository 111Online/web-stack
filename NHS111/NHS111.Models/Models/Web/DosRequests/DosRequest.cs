using Newtonsoft.Json;
using NHS111.Web.Presentation.Models;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosRequest
    {
        [JsonProperty("ServiceVersion")]
        public string ServiceVersion { get { return "1.4"; } }

        [JsonProperty("UserInfo")]
        public DosUserInfo UserInfo { get; protected set; }
    }
}
