using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class PathwaysWhiteListFeature : BaseFeature, IPathwaysWhiteListFeature
    {

        public PathwaysWhiteListFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }

    public interface IPathwaysWhiteListFeature
        : IFeature
    { }

}

