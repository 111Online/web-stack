using System.Configuration;

namespace NHS111.Domain.Configuration
{
    public class Configuration : IConfiguration
    {
        public string GetGraphDbUrl()
        {
            return ConfigurationManager.AppSettings["GraphDbUrl"];
        }
        public string GetGraphDbUsername()
        {
            return ConfigurationManager.AppSettings["GraphDbUsername"];
        }
        public string GetGraphDbPassword()
        {
            return ConfigurationManager.AppSettings["GraphDbPassword"];
        }
    }

    public interface IConfiguration
    {
        string GetGraphDbUrl();
        string GetGraphDbUsername();
        string GetGraphDbPassword();
    }
}