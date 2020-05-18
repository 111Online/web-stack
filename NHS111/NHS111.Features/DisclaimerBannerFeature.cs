
using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public interface IDisclaimerBannerFeature
    {
        bool IsEnabled { get; }
    }

    public class DisclaimerBannerFeature
        : BaseFeature, IDisclaimerBannerFeature
    {
        public DisclaimerBannerFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }
}
