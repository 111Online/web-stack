using System.Configuration;
using NHS111.Features.Defaults;

namespace NHS111.Features.Providers
{
    public class AppSettingStringValueProvider : IFeatureSettingValueProvider<string>
    {
        public string GetSetting(IFeature feature, IDefaultSettingStrategy<string> defaultStrategy, string propertyName)
        {
            var settingName = string.Format("{0}{1}", feature.GetType().Name, propertyName);
            var setting = ConfigurationManager.AppSettings[settingName];

            if (setting != null)
                return setting.ToLower();

            if (defaultStrategy == null)
                throw new MissingSettingException();

            return defaultStrategy.GetDefaultSetting();
        }
    }
}
