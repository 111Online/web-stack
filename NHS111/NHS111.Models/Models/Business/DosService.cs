using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Business
{
    public class DosService : Web.FromExternalServices.DosService
    {
        [JsonProperty(PropertyName = "onlineDosServiceType")]
        public OnlineDOSServiceType OnlineDOSServiceType { get; set; }
    }
}
