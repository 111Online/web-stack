
namespace NHS111.Web.Presentation.Features {
    using Utils.FeatureToggle;

    public interface IDisclaimerPopupFeature {
        bool IsEnabled { get; }
    }

    public class DisclaimerPopupFeature
        : BaseFeature, IDisclaimerPopupFeature {
        public DisclaimerPopupFeature() {
            DefaultSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }
}