using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class DOSFilteringToggleFeature : BaseFeature, IDOSFilteringToggleFeature {

        public DOSFilteringToggleFeature() {
            DefaultBoolSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }

    public interface IDOSFilteringToggleFeature
        : IFeature { }

}
