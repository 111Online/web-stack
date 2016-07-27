namespace NHS111.Utils.FeatureToggle {
    using System.Configuration;

    public class AppSettingValueProvider
        : IFeatureSettingValueProvider
    {

        public bool GetSetting(IFeature feature, IDefaultSettingStrategy defaultStrategy)
        {
            var setting = ConfigurationManager.AppSettings[feature.GetType().Name];

            if (setting != null)
                return setting.ToLower() == "true";

            if (defaultStrategy == null)
                throw new MissingSettingException();

            return defaultStrategy.GetDefaultSetting();
        }
    }
}