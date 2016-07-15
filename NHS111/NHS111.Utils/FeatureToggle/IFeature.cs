namespace NHS111.Utils.FeatureToggle {
    public interface IFeature {
        bool IsEnabled { get; }
    }
}