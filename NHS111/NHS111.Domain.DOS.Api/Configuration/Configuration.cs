using System.Configuration;
using System.Net;

namespace NHS111.Domain.DOS.Api.Configuration
{
    public class Configuration : IConfiguration
    {
        public string DOSIntegrationBaseUrl { get { return ConfigurationManager.AppSettings["DOSIntegrationBaseUrl"]; } }
        public string DOSMobileIntegrationBaseUrl { get { return ConfigurationManager.AppSettings["DOSMobileIntegrationBaseUrl"]; } }
        public NetworkCredential DOSMobileIntegrationCredentials { get
        {
            return new NetworkCredential
            {
                Domain = ConfigurationManager.AppSettings["DOSMobileIntegrationBaseUrl"],
                UserName = ConfigurationManager.AppSettings["DOSMobileIntegrationUser"],
                Password = ConfigurationManager.AppSettings["DOSMobileIntegrationPassword"],
            };
        } }

        public string DOSIntegrationCheckCapacitySummaryUrl {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DOSIntegrationBaseUrl"],
                  ConfigurationManager.AppSettings["DOSIntegrationCheckCapacitySummaryUrl"]);

            }
        }

        public string DOSIntegrationServiceDetailsByIdUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DOSIntegrationBaseUrl"],
                  ConfigurationManager.AppSettings["DOSIntegrationServiceDetailsByIdUrl"]);

            }
        }

        public string DOSIntegrationMonitorHealthUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DOSIntegrationBaseUrl"],
                  ConfigurationManager.AppSettings["DOSIntegrationMonitorHealthUrl"]);

            }
        }

        public string DOSMobileIntegrationServicesByClinicalTermUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DOSMobileIntegrationBaseUrl"],
                  ConfigurationManager.AppSettings["DOSMobileIntegrationServicesByClinicalTermUrl"]);
            }
        }
    }
}