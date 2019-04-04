
using NHS111.Features.Defaults;

namespace NHS111.Features {
    public class HeadlessTestFeature
        : BaseFeature, IHeadlessTestFeature {

        public HeadlessTestFeature() {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }

    public interface IHeadlessTestFeature
        : IFeature { }
}