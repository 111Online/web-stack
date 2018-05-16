using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.Configuration;
using RestSharp;
using NHS111.Models.Models.Business;
using NHS111.Utils.RestTools;
using NUnit.Framework.Internal;
using SimpleJson;
using SimpleJson = SimpleJson.SimpleJson;


namespace NHS111.Business.DOS.Service
{
    public static class PublicHolidaysDataService
    {
        private static PublicHolidaysData _holidays;
        private static IRestClient _publicHolidaysServiceRestClient;
        public static PublicHolidaysData GetPublicHolidays(IConfiguration configuration, ILog logger)
        {
            if (_holidays != null) return _holidays;
            var restclient =  new LoggingRestClient(configuration.PublicHolidaysServiceUri.GetLeftPart(UriPartial.Authority), logger);
            var response =
                restclient.Execute(new RestRequest(configuration.PublicHolidaysServiceUri.PathAndQuery, Method.GET));
            if (response.IsSuccessful)
            {
                JObject jsonData = JObject.Parse(response.Content);
                var holidays = jsonData["england-and-wales"]["events"].ToObject<List<PublicHoliday>>();
                holidays.AddRange(LoadTestHolidays(configuration));
                _holidays = new PublicHolidaysData(holidays);

            }

            return _holidays;
        }

        private static List<PublicHoliday> LoadTestHolidays(IConfiguration configuration)
        {
            var testHolidays = new List<PublicHoliday>();
            if (configuration.TestPublicHolidayDates.Length > 0)
            {
                try
                {

                    var dates = configuration.TestPublicHolidayDates.Split(',');
                    foreach (var date in dates)
                    {
                        testHolidays.Add(new PublicHoliday()
                        {
                            Date = DateTime.ParseExact(date, "dd-MM-yyyy", null),
                            Title = "TEST_HOLIDAY"
                        });
                    }
                }
                finally
                {
                }
            }
            return testHolidays;
        }
    }

    public class PublicHolidaysData : IPublicHolidayData
    {
        private  List<PublicHoliday> _holidays;
        public PublicHolidaysData(List<PublicHoliday> holidays)
        {
            _holidays = holidays;
        }

        public IEnumerable<PublicHoliday> PublicHolidays
        {
            get { return _holidays; }
        }
    }
}
