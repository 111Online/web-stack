using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public interface ICookieBannerFeature
    : IFeature
    {

    }

    public class CookieBannerFeature
        : BaseFeature, ICookieBannerFeature
    {

        public CookieBannerFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }
}