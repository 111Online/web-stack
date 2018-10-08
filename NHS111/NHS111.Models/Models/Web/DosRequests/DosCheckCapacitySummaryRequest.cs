using Newtonsoft.Json;
using NHS111.Web.Presentation.Models;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosCheckCapacitySummaryRequest : DosRequest
    {

        public DosCheckCapacitySummaryRequest(string userName, string password, DosCase dosCase)
        {
            UserInfo = new DosUserInfo(userName, password);
            this.Case = dosCase;
        }

        [JsonProperty("c")]
        public DosCase Case { get; private set; }
    }
}
