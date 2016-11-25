using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Utils.FeatureToggle
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
