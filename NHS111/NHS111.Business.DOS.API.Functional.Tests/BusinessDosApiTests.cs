using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Business.DOS.API.Functional.Tests
{
    using NHS111.Functional.Tests.Tools;
    using System.Configuration;
    using Utils.Helpers;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class BusinessDosApiTests
    {
        private IRestClient _restClient = new RestClient(ConfigurationManager.AppSettings["BusinessDosApiBaseUrl"]);

        private static string BusinessDosCheckCapacitySummaryUrl
        {
            get { return ConfigurationManager.AppSettings["BusinessDosCheckCapacitySummaryUrl"]; }
        }

        private static string BusinessDosServiceDetailsByIdUrl
        {
            get { return ConfigurationManager.AppSettings["BusinessDosServiceDetailsByIdUrl"]; }
        }
        
        private static string DOSApiUsername
        {
            get { return ConfigurationManager.AppSettings["dos_credential_user"]; }
        }

        private static string DOSApiPassword
        {
            get { return ConfigurationManager.AppSettings["dos_credential_password"]; }
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            _restClient.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
        }

        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestCheckDoSBusinessCapacitySumary()
        {
            var dosFilteredCase = new DosFilteredCase {PostCode = "HP21 8AL", Age = "32", Gender = "M", Disposition = 1005, SymptomDiscriminatorList = new []{4460}, SymptomGroup = 1064 };
            var request = new JsonRestRequest(BusinessDosCheckCapacitySummaryUrl, Method.POST);
            request.AddJsonBody(dosFilteredCase);
            var result = await _restClient.ExecuteTaskAsync<DosCheckCapacitySummaryResult>(request);
            Assert.IsTrue(result.IsSuccessful);

            var firstService = result.Data.Success.Services[0];
            AssertResponse(firstService);
        }

        private void AssertResponse(ServiceViewModel response)
        {
            var serviceTypeField = response.ServiceType;
            Assert.IsNotNull(serviceTypeField.Id);
            Assert.IsNotNull(serviceTypeField.Name);

            Assert.IsNotNull(response.Id);
            Assert.IsNotNull(response.Capacity);
            Assert.IsNotNull(response.Name);
            Assert.IsNotNull(response.ContactDetails);
            Assert.IsNotNull(response.Address);
            Assert.IsNotNull(response.PostCode);
            Assert.IsNotNull(response.Northings);
            Assert.IsNotNull(response.Northings);
            Assert.IsNotNull(response.Eastings);
            Assert.IsNotNull(response.Eastings);
            Assert.IsNotNull(response.Url);
            Assert.IsNotNull(response.Notes);
            Assert.IsNotNull(response.OpenAllHours);
            Assert.IsNotNull(response.RotaSessions);
            Assert.IsNotNull(response.ServiceType);
            Assert.IsNotNull(response.OdsCode);
        }

        [Test]
        public async void TestCheckDosBusinessServiceDetailsById()
        {
            var dosServiceRequest = new DosServiceDetailsByIdRequest(DOSApiUsername, DOSApiPassword, "1315835856");
            var request = new JsonRestRequest(BusinessDosServiceDetailsByIdUrl, Method.POST);
            request.AddJsonBody(dosServiceRequest);
            var result = await _restClient.ExecuteTaskAsync<ServiceDetailsByIdResponse>(request);
            
            Assert.IsTrue(result.IsSuccessful);
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.CheckServiceDetailsById);
        }
    }
}
