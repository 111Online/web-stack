using NHS111.Features.Defaults;

namespace NHS111.Features {
    using System.Configuration;

    public interface IAdobeAnalyticsFeature
    : IFeature {

    }

    public class AdobeAnalyticsFeature
        : BaseFeature, IAdobeAnalyticsFeature {

        public AdobeAnalyticsFeature() {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }

        public override bool IsEnabled {
            get {
                return base.IsEnabled && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["AdobeAnalyticsURL"]);
            }
        }
    }
}