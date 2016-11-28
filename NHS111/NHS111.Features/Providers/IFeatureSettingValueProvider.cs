using NHS111.Features.Defaults;

namespace NHS111.Features.Providers {
    public interface IFeatureSettingValueProvider<T> {
        T GetSetting(IFeature feature, IDefaultSettingStrategy<T> defaultStrategy, string propertyName);
    }
}