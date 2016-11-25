namespace NHS111.Utils.FeatureToggle {
    using System.Configuration;

    public class AppSettingBoolValueProvider : IFeatureSettingValueProvider<bool>
    {
        public bool GetSetting(IFeature feature, IDefaultSettingStrategy<bool> defaultStrategy)
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