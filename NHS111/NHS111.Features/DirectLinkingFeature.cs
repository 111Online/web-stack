
using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class DirectLinkingFeature
        : BaseFeature, IDirectLinkingFeature
    {

        public DirectLinkingFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }

    public interface IDirectLinkingFeature
        : IFeature
    { }
}