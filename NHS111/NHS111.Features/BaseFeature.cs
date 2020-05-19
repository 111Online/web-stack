using NHS111.Features.Defaults;
using NHS111.Features.Providers;
using NHS111.Features.Values;

namespace NHS111.Features
{

    public abstract class BaseFeature : IFeature
    {

        protected BaseFeature()
        {
            SettingValueProvider = new AppSettingValueProvider();
        }

        protected BaseFeature(IFeatureSettingValueProvider settingValueProvider)
        {
            SettingValueProvider = settingValueProvider;
        }

        public virtual IFeatureValue FeatureValue(string featureName)
        {
            return FeatureValue(null, featureName);
        }

        public virtual IFeatureValue FeatureValue(IDefaultSettingStrategy defaultSettingStrategy, string featureName)
        {
            return SettingValueProvider.GetSetting(this, defaultSettingStrategy, featureName);
        }

        public virtual bool IsEnabled
        {
            get { return FeatureValue(DefaultIsEnabledSettingStrategy, "IsEnabled").Value.ToLower() == "true"; }
        }

        public IFeatureSettingValueProvider SettingValueProvider { get; set; }

        public IDefaultSettingStrategy DefaultIsEnabledSettingStrategy { get; set; }
    }
}
