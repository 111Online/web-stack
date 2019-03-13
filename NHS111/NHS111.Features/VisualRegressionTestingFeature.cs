using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public interface IVisualRegressionTestingFeature : IFeature
    {
        bool MakeBaselineScreenShotsOnly { get; }
    }

    public class VisualRegressionTestingFeature : BaseFeature, IVisualRegressionTestingFeature
    {
        public VisualRegressionTestingFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }

        public bool MakeBaselineScreenShotsOnly
        {
            get { return bool.Parse(FeatureValue(new DisabledByDefaultSettingStrategy(), "MakeBaselineScreenShotsOnly").Value); }
        }
    }
}
