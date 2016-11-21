using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.DosRequests;

namespace NHS111.Web.Presentation.Models
{
    public class DosCheckCapacitySummaryRequest
    {

        public DosCheckCapacitySummaryRequest(string userName, string password, DosCase dosCase)
        {
            this.UserInfo = new DosUserInfo(userName, password);
            this.Case = dosCase;
        }

        public string ServiceVersion { get { return "1.3"; } }

        public DosUserInfo UserInfo { get; private set; }

        [JsonProperty("c")]
        public DosCase Case { get; private set; }
    }
}
