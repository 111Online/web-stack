namespace NHS111.Features.Defaults
{

    public class DisabledByDefaultSettingStrategy : IDefaultSettingStrategy
    {
        public string Value
        {
            get { return "false"; }
        }
    }
}