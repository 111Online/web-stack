namespace NHS111.Features.Defaults {

    public class DisabledByDefaultSettingStrategy: IDefaultSettingStrategy<bool>
    {

        public bool GetDefaultSetting() {
            return false;
        }
    }
}