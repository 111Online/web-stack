
namespace NHS111.Web.Views.Shared {
    using System.Web.Mvc;
    using Presentation.Features;

    public class DisclaimerPopupView
        : WebViewPage {

        public IDisclaimerPopupFeature DisclaimerPopupFeature { get; set; }

        public DisclaimerPopupView() {
            DisclaimerPopupFeature = new DisclaimerPopupFeature();
        }

        public override void Execute() { }
    }
}