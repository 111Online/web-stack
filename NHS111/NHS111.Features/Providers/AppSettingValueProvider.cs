using NHS111.Features.Defaults;
using NHS111.Features.Values;
using System.Configuration;

namespace NHS111.Features.Providers
{
    public class AppSettingValueProvider : IFeatureSettingValueProvider
    {
        public IFeatureValue GetSetting(IFeature feature, IDefaultSettingStrategy defaultStrategy, string propertyName)
        {
            var settingName = string.Format("{0}{1}", feature.GetType().Name, propertyName);
            var setting = ConfigurationManager.AppSettings[settingName];

            if (setting != null)
                return new FeatureValue(setting);

            if (defaultStrategy == null)
                throw new MissingSettingException("Missing setting : " + settingName);

            return new FeatureValue(defaultStrategy.Value);
        }
    }
}