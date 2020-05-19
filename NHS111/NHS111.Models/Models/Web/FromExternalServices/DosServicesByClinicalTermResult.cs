using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class DosServicesByClinicalTermResult
    {
        [JsonProperty(PropertyName = "success")]
        public SuccessObject<MobileDosService> Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ErrorObject Error { get; set; }
    }
}
