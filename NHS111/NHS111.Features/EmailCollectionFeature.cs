using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public interface IEmailCollectionFeature : IFeature { }

    public class EmailCollectionFeature : BaseFeature, IEmailCollectionFeature
    {
        public EmailCollectionFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }
}
