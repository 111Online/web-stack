namespace NHS111.Web.Views.Shared {
    using Presentation.Features;

    public class LayoutView
        : DisclaimerPopupView {

        public IDisclaimerBannerFeature DisclaimerBannerFeature { get; set; }
        public ICookieBannerFeature CookieBannerFeature { get; set; }

        public LayoutView() {
            DisclaimerBannerFeature = new DisclaimerBannerFeature();
            CookieBannerFeature = new CookieBannerFeature();
        }
    }
}