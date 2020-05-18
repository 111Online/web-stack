
using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public interface IDisclaimerPopupFeature
    {
        bool IsEnabled { get; }
    }

    public class DisclaimerPopupFeature
        : BaseFeature, IDisclaimerPopupFeature
    {
        public DisclaimerPopupFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }
}