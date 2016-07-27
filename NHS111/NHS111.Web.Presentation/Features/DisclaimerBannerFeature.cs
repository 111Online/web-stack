
namespace NHS111.Web.Presentation.Features {
    using Utils.FeatureToggle;

    public interface IDisclaimerBannerFeature {
        bool IsEnabled { get; }
    }

    public class DisclaimerBannerFeature
        : BaseFeature, IDisclaimerBannerFeature {
        public DisclaimerBannerFeature() {
            DefaultSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }
}
