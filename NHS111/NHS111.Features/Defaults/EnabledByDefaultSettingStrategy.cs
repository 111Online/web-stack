
namespace NHS111.Features.Defaults
{

    public class EnabledByDefaultSettingStrategy : IDefaultSettingStrategy
    {

        public string Value
        {
            get { return "true"; }
        }
    }
}