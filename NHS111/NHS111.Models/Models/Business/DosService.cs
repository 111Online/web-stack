using Newtonsoft.Json;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Business
{
    public class DosService : Web.FromExternalServices.DosService
    {
        public DosService()
        { }
        public DosService(IClock clock) : base(clock)
        { }

        [JsonProperty(PropertyName = "onlineDosServiceType")]
        public OnlineDOSServiceType OnlineDOSServiceType
        { get; set; }
    }
}