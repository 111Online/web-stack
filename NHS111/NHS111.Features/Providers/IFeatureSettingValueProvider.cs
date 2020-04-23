using NHS111.Features.Defaults;
using NHS111.Features.Values;

namespace NHS111.Features.Providers
{
    public interface IFeatureSettingValueProvider
    {
        IFeatureValue GetSetting(IFeature feature, IDefaultSettingStrategy defaultStrategy, string propertyName);
    }
}