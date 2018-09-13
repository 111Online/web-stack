using Newtonsoft.Json;
using NHS111.Web.Presentation.Models;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosCheckCapacitySummaryRequest
    {

        public DosCheckCapacitySummaryRequest(string userName, string password, DosCase dosCase)
        {
            this.UserInfo = new DosUserInfo(userName, password);
            this.Case = dosCase;
        }

        public string ServiceVersion { get { return "1.4"; } }

        public DosUserInfo UserInfo { get; private set; }

        [JsonProperty("c")]
        public DosCase Case { get; private set; }
    }
}
