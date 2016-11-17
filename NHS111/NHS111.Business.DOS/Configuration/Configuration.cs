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
    }
}
