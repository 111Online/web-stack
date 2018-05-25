using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NHS111.Business.DOS.Configuration;
using RestSharp;
using NHS111.Models.Models.Business;
using NHS111.Utils.RestTools;


namespace NHS111.Business.DOS.Test.Service
{
    public static class PublicHolidaysData
    {
        private static List<PublicHoliday> _holidays;
        public static List<PublicHoliday> GetPublicHolidays(IConfiguration configuration)
        {
            if (_holidays != null) return _holidays;
            var restclient =  new LoggingRestClient(configuration.PublicHolidaysServiceUri.Authority, LogManager.GetLogger("log"));
            var response = restclient.ExecuteTaskAsync<List<PublicHoliday>>(
                new RestRequest(configuration.PublicHolidaysServiceUri.PathAndQuery, Method.GET));
            _holidays = response.Result.Data;
            return _holidays;

        }
    }
}
