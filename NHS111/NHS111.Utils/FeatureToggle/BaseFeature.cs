
namespace NHS111.Utils.FeatureToggle {

    public abstract class BaseFeature : IFeature {

        protected BaseFeature() {
            BoolSettingValueProvider = new AppSettingBoolValueProvider();
            StringSettingValueProvider = new AppSettingStringValueProvider();
        }

        protected BaseFeature(IFeatureSettingValueProvider<bool> boolSettingValueProvider, IFeatureSettingValueProvider<string> stringSettingValueProvider) {
            BoolSettingValueProvider = boolSettingValueProvider;
            StringSettingValueProvider = stringSettingValueProvider;
        }

        public virtual bool IsEnabled {
            get { return BoolSettingValueProvider.GetSetting(this, DefaultBoolSettingStrategy); }
        }

        public virtual string StringValue
        {
            get { return StringSettingValueProvider.GetSetting(this, DefaultStringSettingStrategy); }
        }

        public IFeatureSettingValueProvider<bool> BoolSettingValueProvider { get; set; }
        public IDefaultSettingStrategy<bool> DefaultBoolSettingStrategy { get; set; }
        public IFeatureSettingValueProvider<string> StringSettingValueProvider { get; set; }
        public IDefaultSettingStrategy<string> DefaultStringSettingStrategy { get; set; }
    }
}
