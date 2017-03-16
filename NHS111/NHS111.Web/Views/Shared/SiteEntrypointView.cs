
using NHS111.Features;
using NHS111.Models.Models.Web;

namespace NHS111.Web.Views.Shared {

    public class SiteEntrypointView
        : BaseView<JourneyViewModel>
    {

        public IDisclaimerPopupFeature DisclaimerPopupFeature { get; set; }

        public SiteEntrypointView()
        {
            DisclaimerPopupFeature = new DisclaimerPopupFeature();
        }

        public override void Execute() { }
    }
}