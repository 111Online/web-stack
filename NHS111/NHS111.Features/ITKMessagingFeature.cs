using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class ITKMessagingFeature
        : BaseFeature, IITKMessagingFeature
    {

        public ITKMessagingFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }

    public interface IITKMessagingFeature
        : IFeature
    { }
}