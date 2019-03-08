using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public interface IVisualRegressionTestingFeature : IFeature
    {
    }

    public class VisualRegressionTestingFeature : BaseFeature, IVisualRegressionTestingFeature
    {
        public VisualRegressionTestingFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }
}
