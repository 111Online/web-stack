using NHS111.Features.Defaults;

namespace NHS111.Features
{

    public interface IRedirectToStartFeature : IFeature
    {
        string RedirectUrl { get; }
    }

    public class RedirectToStartFeature : BaseFeature, IRedirectToStartFeature
    {
        public RedirectToStartFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }

        public string RedirectUrl
        {
            get { return FeatureValue("RedirectUrl").Value; }
        }
    }
}
