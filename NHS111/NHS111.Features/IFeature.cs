using NHS111.Features.Defaults;
using NHS111.Features.Values;

namespace NHS111.Features
{
    public interface IFeature
    {
        bool IsEnabled { get; }
        IFeatureValue FeatureValue(string featureName);
        IFeatureValue FeatureValue(IDefaultSettingStrategy defaultSettingStrategy, string featureName);
    }
}