using NHS111.Features;
using NHS111.Models.Models.Web;

namespace NHS111.Web.Views.Shared {
    public class LayoutView
        : BaseView<JourneyViewModel> {
        public IDisclaimerBannerFeature DisclaimerBannerFeature { get; set; }
        public IDisclaimerPopupFeature DisclaimerPopupFeature { get; set; }
        public ICookieBannerFeature CookieBannerFeature { get; set; }

        public LayoutView() {
            DisclaimerBannerFeature = new DisclaimerBannerFeature();
            DisclaimerPopupFeature = new DisclaimerPopupFeature();
            CookieBannerFeature = new CookieBannerFeature();
        }
    }
}