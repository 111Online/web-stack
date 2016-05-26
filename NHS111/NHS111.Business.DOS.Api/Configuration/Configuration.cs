using System.Configuration;

namespace NHS111.Business.DOS.Api.Configuration
{
    public class Configuration : IConfiguration
    {
        public string DomainDOSApiBaseUrl { get { return ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"]; } }
        public string DomainDOSApiCheckCapacitySummaryUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"],
                  ConfigurationManager.AppSettings["DomainDOSApiCheckCapacitySummaryUrl"]);
            }
        }

        public string DomainDOSApiServiceDetailsByIdUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"],
                  ConfigurationManager.AppSettings["DomainDOSApiServiceDetailsByIdUrl"]);
            }
        }

        public string DomainDOSApiMonitorHealthUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"],
                  ConfigurationManager.AppSettings["DomainDOSApiMonitorHealthUrl"]);

            }
        }

        public string DomainDOSApiServicesByClinicalTermUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"],
                    ConfigurationManager.AppSettings["DomainDOSApiServicesByClinicalTermUrl"]);
            }
        }
    }
}