namespace NHS111.Features.Defaults
{
    public class FilteredAgesDefaultStrategy : IDefaultSettingStrategy<string>
    {
        public string GetDefaultSetting()
        {
            // "infant|toddler"
            return string.Empty;
        }
    }
}
