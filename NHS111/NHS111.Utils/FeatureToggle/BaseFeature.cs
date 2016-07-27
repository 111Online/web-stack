
namespace NHS111.Utils.FeatureToggle {

    public abstract class BaseFeature
        : IFeature {

        protected BaseFeature() {
            SettingValueProvider = new AppSettingValueProvider();
        }

        protected BaseFeature(IFeatureSettingValueProvider settingValueProvider) {
            SettingValueProvider = settingValueProvider;
        }

        public virtual bool IsEnabled {
            get { return SettingValueProvider.GetSetting(this, DefaultSettingStrategy); }
        }

        public IFeatureSettingValueProvider SettingValueProvider { get; set; }
        public IDefaultSettingStrategy DefaultSettingStrategy { get; set; }
    }
}
