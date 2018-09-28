using Newtonsoft.Json;
using NHS111.Web.Presentation.Models;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosServiceDetailsByIdRequest : DosRequest
    {
        public DosServiceDetailsByIdRequest(string userName, string password, string serviceId)
        {
            UserInfo = new DosUserInfo(userName, password);
            this.ServiceId = serviceId;
        }

        [JsonProperty("serviceId")]
        public string ServiceId { get; private set; }
    }
}
