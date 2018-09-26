using NHS111.Features.Defaults;

namespace NHS111.Features {
    public interface IAdobeAnalyticsFeature
    : IFeature {

    }

    public class AdobeAnalyticsFeature
        : BaseFeature, IAdobeAnalyticsFeature {

        public AdobeAnalyticsFeature() {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }
}