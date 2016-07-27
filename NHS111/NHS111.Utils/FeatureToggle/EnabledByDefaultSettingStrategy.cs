
namespace NHS111.Utils.FeatureToggle {

    public class EnabledByDefaultSettingStrategy
        : IDefaultSettingStrategy {

        public bool GetDefaultSetting() {
            return true;
        }
    }
}