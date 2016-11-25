namespace NHS111.Utils.FeatureToggle {

    public class DisabledByDefaultSettingStrategy: IDefaultSettingStrategy<bool>
    {

        public bool GetDefaultSetting() {
            return false;
        }
    }
}