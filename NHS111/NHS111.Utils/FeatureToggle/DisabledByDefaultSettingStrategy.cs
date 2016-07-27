namespace NHS111.Utils.FeatureToggle {

    public class DisabledByDefaultSettingStrategy
        : IDefaultSettingStrategy {

        public bool GetDefaultSetting() {
            return false;
        }
    }
}