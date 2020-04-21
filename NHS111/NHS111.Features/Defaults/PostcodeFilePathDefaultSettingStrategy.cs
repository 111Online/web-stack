namespace NHS111.Features.Defaults
{
    public class PostcodeFilePathDefaultSettingStrategy : IDefaultSettingStrategy
    {
        public string Value { get { return @"postcodes.csv"; } }
    }
}
