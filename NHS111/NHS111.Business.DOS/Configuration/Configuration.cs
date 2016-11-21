using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web.FromExternalServices;
using NodaTime;
using NodaTime.Text;

namespace NHS111.Business.DOS.Configuration
{
    public class Configuration : IConfiguration
    {

        public LocalTime WorkingDayInHoursStartTime
        {
            get { return Get(ConfigurationManager.AppSettings["WorkingDayInHoursStartTime"]); }
        }

        public LocalTime WorkingDayInHoursEndTime
        {
            get { return Get(ConfigurationManager.AppSettings["WorkingDayInHoursEndTime"]); }
        }

        public LocalTime WorkingDayInHoursShoulderEndTime
        {
            get { return Get(ConfigurationManager.AppSettings["WorkingDayInHoursShoulderEndTime"]); }
        }

        private LocalTime Get(string configText)
        {
           var parser =  LocalTimePattern.CreateWithCurrentCulture("HH:mm").Parse(configText);
           return parser.GetValueOrThrow();
        }
        public string DomainDosApiBaseUrl { get { return ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"]; } }
        public string DomainDosApiCheckCapacitySummaryUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"],
                  ConfigurationManager.AppSettings["DomainDOSApiCheckCapacitySummaryUrl"]);
            }
        }

        public string DomainDosApiServiceDetailsByIdUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"],
                  ConfigurationManager.AppSettings["DomainDOSApiServiceDetailsByIdUrl"]);
            }
        }

        public string DomainDosApiMonitorHealthUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"],
                  ConfigurationManager.AppSettings["DomainDOSApiMonitorHealthUrl"]);

            }
        }

        public string DomainDosApiServicesByClinicalTermUrl
        {
            get
            {
                return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"],
                    ConfigurationManager.AppSettings["DomainDOSApiServicesByClinicalTermUrl"]);
            }
        }

        public string FilteredDispositionCodes
        {
            get { return ConfigurationManager.AppSettings["FilteredDispositionCodes"]; }
        }

        public string FilteredDosServiceIds
        {
            get { return ConfigurationManager.AppSettings["FilteredDosServiceIds"]; }
        }

        public string DosUsername { get { return ConfigurationManager.AppSettings["dos_credential_user"]; } }
        public string DosPassword { get { return ConfigurationManager.AppSettings["dos_credential_password"]; } }
    }
}
