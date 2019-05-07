using System.Configuration;
using Newtonsoft.Json.Linq;
using NHS111.Functional.Tests.Tools;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using NHS111.Utils.RestTools;
using NUnit.Framework;
using RestSharp;

namespace NHS111.DOS.Domain.API.Functional.Tests
{
    [TestFixture]
    public class DomainDOSApiTests
    {
        private static string DomainDOSApiCheckCapacitySummaryUrl
        {
            get { return ConfigurationManager.AppSettings["DomainDOSApiCheckCapacitySummaryUrl"]; }
        }

        private static string DomainDOSApiServiceDetailsByIdUrl
        {
            get { return ConfigurationManager.AppSettings["DomainDOSApiServiceDetailsByIdUrl"]; }
        }

        private static string DomainDOSApiServicesByClinicalTermUrl
        {
            get { return ConfigurationManager.AppSettings["DomainDOSApiServicesByClinicalTermUrl"]; }
        }
        
        private static string DOSApiUsername
        {
            get { return ConfigurationManager.AppSettings["dos_credential_user"]; }
        }

        private static string DOSApiPassword
        {
            get { return ConfigurationManager.AppSettings["dos_credential_password"]; }
        }

        private IRestClient _restClient = new RestClient(ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"]);

        [TestFixtureSetUp]
        public void SetUp()
        {
            _restClient.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
        }

        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestCheckCapacitySumary()
        {
            var checkCapacitySummaryRequest = new DosCheckCapacitySummaryRequest(DOSApiUsername, DOSApiPassword, new DosCase { Age = "22", Gender = "F", PostCode = "HP21 8AL" });
            var request = new JsonRestRequest(DomainDOSApiCheckCapacitySummaryUrl, Method.POST);
            request.AddJsonBody(checkCapacitySummaryRequest);
            var result = await _restClient.ExecuteTaskAsync<DosCheckCapacitySummaryResult>(request);
            Assert.IsTrue(result.IsSuccessful);

            var firstService = result.Data.Success.Services[0];
            AssertResponse(firstService);
        }

        private void AssertResponse(ServiceViewModel response)
        {
            var serviceTypeField = response.ServiceType;
            Assert.IsNotNull(serviceTypeField.Id);
            Assert.IsNotNull(serviceTypeField.ContactDetails[0].Name);

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
        public async void TestCheckServiceDetailsById()
        {
            var serviceDetailsByIdRequest = new DosServiceDetailsByIdRequest(DOSApiUsername, DOSApiPassword, "1315835856");
            var request = new JsonRestRequest(DomainDOSApiServiceDetailsByIdUrl, Method.POST);
            request.AddJsonBody(serviceDetailsByIdRequest);
            var result = await _restClient.ExecuteTaskAsync<ServiceDetailsByIdResponse>(request);

            Assert.IsTrue(result.IsSuccessful);
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.CheckServiceDetailsById);
        }
    }
}
