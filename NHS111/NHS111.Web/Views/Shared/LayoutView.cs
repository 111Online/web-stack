using System.Web.Mvc;
using NHS111.Features;
using NHS111.Models.Models.Web;

namespace NHS111.Web.Views.Shared
{
    public class LayoutView
        : WebViewPage<JourneyViewModel>
    {
        public IDisclaimerBannerFeature DisclaimerBannerFeature { get; set; }
        public IDisclaimerPopupFeature DisclaimerPopupFeature { get; set; }
        public ICookieBannerFeature CookieBannerFeature { get; set; }
        public IUserZoomSurveyFeature UserZoomSurveyFeature { get; set; }
        public IAdobeAnalyticsFeature AdobeAnalyticsFeature { get; set; }

        public LayoutView()
        {
            DisclaimerBannerFeature = new DisclaimerBannerFeature();
            DisclaimerPopupFeature = new DisclaimerPopupFeature();
            CookieBannerFeature = new CookieBannerFeature();
            UserZoomSurveyFeature = new UserZoomSurveyFeature();
            AdobeAnalyticsFeature = new AdobeAnalyticsFeature();
        }

        public override void Execute() { }
    }
}