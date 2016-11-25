using NHS111.Utils.FeatureToggle;

namespace NHS111.Domain.Features
{
    public class PathwaysWhiteListFeature : BaseFeature, IPathwaysWhiteListFeature
    {

        public PathwaysWhiteListFeature()
        {
            DefaultBoolSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }

    public interface IPathwaysWhiteListFeature
        : IFeature { }
    
}

