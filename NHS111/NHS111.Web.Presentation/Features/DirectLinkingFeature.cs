
namespace NHS111.Web.Presentation.Features {
    using Utils.FeatureToggle;

    public class DirectLinkingFeature
        : BaseFeature, IDirectLinkingFeature {

        public DirectLinkingFeature() {
            DefaultBoolSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }

    public interface IDirectLinkingFeature
        : IFeature { }
}