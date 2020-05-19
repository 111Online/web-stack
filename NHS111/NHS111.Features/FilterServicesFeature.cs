using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class FilterServicesFeature : BaseFeature, IFilterServicesFeature
    {

        public FilterServicesFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }

    public interface IFilterServicesFeature : IFeature
    { }
}
