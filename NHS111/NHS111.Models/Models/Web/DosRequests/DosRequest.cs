using NHS111.Web.Presentation.Models;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosRequest
    {
        public string ServiceVersion { get { return "1.4"; } }

        public DosUserInfo UserInfo { get; protected set; }
    }
}
