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
