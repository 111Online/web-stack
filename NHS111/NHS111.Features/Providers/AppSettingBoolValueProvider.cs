using System.Configuration;
using NHS111.Features.Defaults;

namespace NHS111.Features.Providers {
    public class AppSettingBoolValueProvider : IFeatureSettingValueProvider<bool>
    {
        public bool GetSetting(IFeature feature, IDefaultSettingStrategy<bool> defaultStrategy, string propertyName)
        {
            var settingName = string.Format("{0}{1}", feature.GetType().Name, propertyName);
            var setting = ConfigurationManager.AppSettings[settingName];

            if (setting != null)
                return setting.ToLower() == "true";

            if (defaultStrategy == null)
                throw new MissingSettingException();

            return defaultStrategy.GetDefaultSetting();
        }
    }
}