namespace NHS111.Utils.FeatureToggle {
    public interface IFeatureSettingValueProvider {
        bool GetSetting(IFeature feature, IDefaultSettingStrategy defaultStrategy);
    }
}