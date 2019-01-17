using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NHS111.Utils.Analytics
{
    public interface IGoogleAnalyticsAPI
    {
        void RaiseEvent(HttpRequest requestContext, EventMeasurement gaEvent);
    }

    public class GoogleAnalyticsAPI : IGoogleAnalyticsAPI
    {
        public const string SourceKey = "utm_source";
        public const string MediumKey = "utm_medium";
        public const string CampaignKey = "utm_campaign";
        public const string ContentKey = "utm_content";
        public const string GACookieName = "_ga";
        public const string GAIDCookieName = "_gid";

        public GoogleAnalyticsAPI(string trackingId, string analyticsHost)
        {
            _trackingId = trackingId;
            _analyticsHost = analyticsHost;
        }

        public void RaiseEvent(HttpRequest requestContext, EventMeasurement gaEvent)
        {
            if (!IsValidContext(requestContext))
                return;

            gaEvent.ClientId = _clientId;
            gaEvent.UserId = _userId;
            gaEvent.TrackingId = _trackingId;

            Task.Factory.StartNew(() => gaEvent.PostToAnalyticsAsync(_analyticsHost));
        }

        private bool IsValidContext(HttpRequest requestContext)
        {
            if (!requestContext.Cookies.AllKeys.Contains(GACookieName) || !requestContext.Cookies.AllKeys.Contains(GAIDCookieName))
                return false;

            _clientId = requestContext.Cookies[GACookieName].Value;
            _userId = requestContext.Cookies[GAIDCookieName].Value;
            return true;
        }

        private readonly string _trackingId;
        private readonly string _analyticsHost;
        private string _clientId;
        private string _userId;
    }
}
