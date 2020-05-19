using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public interface IEmergencyAlertFeature
    {
        bool IsEnabled { get; }
    }

    public class EmergencyAlertFeature
        : BaseFeature, IEmergencyAlertFeature
    {
        public EmergencyAlertFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }
}
