
namespace NHS111.Features.Defaults {

    public class EnabledByDefaultSettingStrategy : IDefaultSettingStrategy<bool> {

        public bool GetDefaultSetting() {
            return true;
        }
    }
}