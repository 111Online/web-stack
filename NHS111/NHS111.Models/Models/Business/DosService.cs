using Newtonsoft.Json;

namespace NHS111.Models.Models.Business
{
    public class DosService : Web.FromExternalServices.DosService
    {
        [JsonProperty(PropertyName = "callbackEnabled")]
        public bool CallbackEnabled { get; set; }
    }
}
