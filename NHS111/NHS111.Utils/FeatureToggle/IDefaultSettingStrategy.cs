namespace NHS111.Utils.FeatureToggle {

    public interface IDefaultSettingStrategy<out T> {
        T GetDefaultSetting();
    }
}