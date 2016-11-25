
namespace NHS111.Utils.FeatureToggle {

    public class EnabledByDefaultSettingStrategy : IDefaultSettingStrategy<bool> {

        public bool GetDefaultSetting() {
            return true;
        }
    }
}