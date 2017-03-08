
using NHS111.Features;

namespace NHS111.Web.Views.Shared {
    using System.Web.Mvc;
    using Models.Models.Web;

    public class SiteEntrypointView
        : WebViewPage<JourneyViewModel> {

        public IDisclaimerPopupFeature DisclaimerPopupFeature { get; set; }
        public IUserZoomSurveyFeature UserZoomSurveyFeature { get; set; }

        public SiteEntrypointView()
        {
            DisclaimerPopupFeature = new DisclaimerPopupFeature();
            UserZoomSurveyFeature = new UserZoomSurveyFeature();
        }

        public override void Execute() { }
    }
}