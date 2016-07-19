
namespace NHS111.Web.Presentation.Features {
    using Utils.FeatureToggle;

    public interface ICookieBannerFeature
    : IFeature {

    }

    public class CookieBannerFeature
        : BaseFeature, ICookieBannerFeature {

        public CookieBannerFeature() {
            DefaultSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }
}