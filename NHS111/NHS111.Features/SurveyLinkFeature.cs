using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class SurveyLinkFeature : BaseFeature, ISurveyLinkFeature
    {
        public SurveyLinkFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }

    public interface ISurveyLinkFeature : IFeature
    {
        
    }
}
