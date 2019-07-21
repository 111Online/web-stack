using System.Configuration;

namespace NHS111.Domain.Configuration
{
    public class Configuration : IConfiguration
    {
        public string GetGraphDbUrl()
        {
            return ConfigurationManager.AppSettings["GraphDbUrl"];
        }

        public string GetPathwaysVersion()
        {
            return ConfigurationManager.AppSettings["PathwaysVersion"];
        }
    }

    public interface IConfiguration
    {
        string GetGraphDbUrl();
        string GetPathwaysVersion();
    }
}