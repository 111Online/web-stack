using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Integration.DOS.Api.DOSService;
using NHS111.Utils.Monitoring;

namespace NHS111.Integration.DOS.Api.Monitoring
{
    using System.Reflection;

    public class Monitor : BaseMonitor
    {
        private readonly IPathwayServiceSoapFactory _pathWayServiceFactory;

        public Monitor(IPathwayServiceSoapFactory pathWayServiceFactory)
        {
            _pathWayServiceFactory = pathWayServiceFactory;
        }

        private static string DosUser
        {
            get { return ConfigurationManager.AppSettings["dos_credential_user"]; }
        }

        private static string DosPassword
        {
            get { return ConfigurationManager.AppSettings["dos_credential_password"]; }
        }

        public override string Metrics()
        {
            return "Metrics";
        }

        public override async Task<bool> Health()
        {
            try
            {
                var jsonString =
                    new StringBuilder("{\"serviceVersion\":\"1.4\",\"userInfo\":{\"username\":\"" + DosUser + "\",\"password\":\"" + DosPassword + "\"},")
                        .Append("\"c\":{\"caseRef\":\"123\",\"caseId\":\"123\",\"postcode\":\"EC1A 4JQ\",\"surgery\":\"")
                        .Append("A83046\",\"age\":35,")
                        .Append("\"ageFormat\":0,\"disposition\":1002")
                        .Append(",\"symptomGroup\":1110,\"symptomDiscriminatorList\":[4052],")
                        .Append("\"searchDistanceSpecified\":false,\"gender\":\"M\"}}").ToString();

                var checkCapacitySummaryRequest = JsonConvert.DeserializeObject<CheckCapacitySummaryRequest>(jsonString);
                var client = _pathWayServiceFactory.Create(new HttpRequestMessage() {RequestUri = new Uri("http://healthcheck.co.uk")});
                var result = await client.CheckCapacitySummaryAsync(checkCapacitySummaryRequest);

                return result != null && result.CheckCapacitySummaryResult.Any();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override string Version() {
            return Assembly.GetCallingAssembly().GetName().Version.ToString();
        }
    }
}