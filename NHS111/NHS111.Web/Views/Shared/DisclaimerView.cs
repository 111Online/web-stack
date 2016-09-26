
namespace NHS111.Web.Views.Shared {
    using System.Web.Mvc;
    using Models.Models.Web;
    using Presentation.Features;

    public class DisclaimerPopupView
        : WebViewPage<JourneyViewModel> {

        public IDisclaimerPopupFeature DisclaimerPopupFeature { get; set; }

        public DisclaimerPopupView() {
            DisclaimerPopupFeature = new DisclaimerPopupFeature();
        }

        public override void Execute() { }
    }
}