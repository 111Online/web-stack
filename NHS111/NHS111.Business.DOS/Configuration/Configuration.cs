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

        public LocalTime WorkingDayPrimaryCareInHoursStartTime
        {
            get { return Get(ConfigurationManager.AppSettings["WorkingDayPrimaryCareInHoursStartTime"]); }
        }

        public LocalTime WorkingDayPrimaryCareInHoursEndTime
        {
            get { return Get(ConfigurationManager.AppSettings["WorkingDayPrimaryCareInHoursEndTime"]); }
        }

        public LocalTime WorkingDayPrimaryCareInHoursShoulderEndTime
        {
            get { return Get(ConfigurationManager.AppSettings["WorkingDayPrimaryCareInHoursShoulderEndTime"]); }
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

        public string FilteredPrimaryCareDispositionCodes
        {
            get { return ConfigurationManager.AppSettings["FilteredPrimaryCareDispositionCodes"]; }
        }

        public string FilteredPrimaryCareDosServiceIds
        {
            get { return ConfigurationManager.AppSettings["FilteredPrimaryCareDosServiceIds"]; }
        }

        public string DosUsername { get { return ConfigurationManager.AppSettings["dos_credential_user"]; } }
        public string DosPassword { get { return ConfigurationManager.AppSettings["dos_credential_password"]; } }


        public string FilteredDentalDispositionCodes
        {
            get { return ConfigurationManager.AppSettings["FilteredDentalDispositionCodes"]; }
        }


        public string FilteredDentalDosServiceIds
        {
            get { return ConfigurationManager.AppSettings["FilteredDentalDosServiceIds"]; }
        }

        public string FilteredClinicianCallbackDispositionCodes
        {
            get { return ConfigurationManager.AppSettings["FilteredClinicianCallbackDispositionCodes"]; }
        }

        public string FilteredClinicianCallbackDosServiceIds
        {
            get { return ConfigurationManager.AppSettings["FilteredClinicianCallbackServiceIds"]; }
        }

        public LocalTime WorkingDayDentalInHoursStartTime
        {
            get { return Get(ConfigurationManager.AppSettings["WorkingDayDentalInHoursStartTime"]); }
        }

        public LocalTime WorkingDayDentalInHoursEndTime
        {
            get { return Get(ConfigurationManager.AppSettings["WorkingDayDentalInHoursEndTime"]); }
        }

        public LocalTime WorkingDayDentalInHoursShoulderEndTime
        {
            get { return Get(ConfigurationManager.AppSettings["WorkingDayDentalInHoursShoulderEndTime"]); }
        }
    }
}
