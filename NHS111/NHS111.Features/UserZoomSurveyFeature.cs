using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public interface IUserZoomSurveyFeature
    {
        bool IsEnabled { get; }
    }

    public class UserZoomSurveyFeature
        : BaseFeature, IUserZoomSurveyFeature
    {
        public UserZoomSurveyFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }
}