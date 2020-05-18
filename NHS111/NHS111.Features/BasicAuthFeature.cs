
namespace NHS111.Features
{
    using Defaults;

    public interface IBasicAuthFeature : IFeature { }

    public class BasicAuthFeature : BaseFeature, IBasicAuthFeature
    {
        public BasicAuthFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }
    }
}