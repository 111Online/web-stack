namespace NHS111.Utils.FeatureToggle {
    public interface IFeatureSettingValueProvider<T> {
        T GetSetting(IFeature feature, IDefaultSettingStrategy<T> defaultStrategy, string propertyName);
    }
}