using NHS111.Features;
using System.Web.Mvc;

namespace NHS111.Web.Views.Shared
{
    public class LayoutView
        : WebViewPage
    {
        public IDisclaimerBannerFeature DisclaimerBannerFeature { get; set; }
        public IDisclaimerPopupFeature DisclaimerPopupFeature { get; set; }
        public ICookieBannerFeature CookieBannerFeature { get; set; }
        public IUserZoomSurveyFeature UserZoomSurveyFeature { get; set; }
        public IEmergencyAlertFeature EmergencyAlertFeature { get; set; }

        public LayoutView()
        {
            DisclaimerBannerFeature = new DisclaimerBannerFeature();
            DisclaimerPopupFeature = new DisclaimerPopupFeature();
            CookieBannerFeature = new CookieBannerFeature();
            UserZoomSurveyFeature = new UserZoomSurveyFeature();
            EmergencyAlertFeature = new EmergencyAlertFeature();
        }

        public override void Execute() { }
    }
}