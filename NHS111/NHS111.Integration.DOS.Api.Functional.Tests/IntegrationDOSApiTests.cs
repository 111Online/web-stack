using System.Linq;
using Newtonsoft.Json;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Integration.DOS.Api.Functional.Tests
{
    using Utils.Helpers;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;
    using NHS111.Functional.Tests.Tools;
    using System.Configuration;

    [TestFixture]
    public class IntegrationDOSApiTests
    {
        private static string DOSApiUsername
        {
            get { return ConfigurationManager.AppSettings["dos_credential_user"]; }
        }

        private static string DOSApiPassword
        {
            get { return ConfigurationManager.AppSettings["dos_credential_password"]; }
        }

        private static string DOSIntegrationCheckCapacitySummaryUrl
        {
            get { return ConfigurationManager.AppSettings["DOSIntegrationCheckCapacitySummaryUrl"]; }
        }

        private static string DOSIntegrationServiceDetailsByIdUrl
        {
            get { return  ConfigurationManager.AppSettings["DOSIntegrationServiceDetailsByIdUrl"]; }
        }

        private IRestClient _restClient = new RestClient(ConfigurationManager.AppSettings["DOSIntegrationBaseUrl"]);


        [TestFixtureSetUp]
        public void SetUp()
        {
            _restClient.AddHandler("application/json", NewtonsoftJsonSerializer.Default);
        }

        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestCheckDosIntegrationCapacitySumary()
        {
            var checkCapacitySummaryRequest = new DosCheckCapacitySummaryRequest(DOSApiUsername, DOSApiPassword, new DosCase { Age = "22", Gender = "F", PostCode = "HP21 8AL" });
            var request = new JsonRestRequest(DOSIntegrationCheckCapacitySummaryUrl, Method.POST);
            request.AddJsonBody(checkCapacitySummaryRequest);
            var result = await _restClient.ExecuteTaskAsync<CheckCapacitySummaryResponse>(request);
            Assert.IsTrue(result.IsSuccessful);

            var firstService = result.Data.CheckCapacitySummaryResult[0];
            AssertResponse(firstService);
        }

        private void AssertResponse(DosService response)
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
            Assert.IsNotNull(response.Eastings);
            Assert.IsNotNull(response.Url);
            Assert.IsNotNull(response.Notes);
            Assert.IsNotNull(response.OpenAllHours);
            Assert.IsNotNull(response.RotaSessions);
            Assert.IsNotNull(response.ServiceType);
            Assert.IsNotNull(response.OdsCode);
        }

        [Test]
        public async void TestCheckDosIntegrationServiceDetailsById()
        {
            var serviceDetailsByIdRequest = new DosServiceDetailsByIdRequest(DOSApiUsername, DOSApiPassword, "1315835856");
            var request = new JsonRestRequest(DOSIntegrationServiceDetailsByIdUrl, Method.POST);
            request.AddJsonBody(serviceDetailsByIdRequest);
            var result = await _restClient.ExecuteTaskAsync<ServiceDetailsByIdResponse>(request);

            Assert.IsTrue(result.IsSuccessful);
            SchemaValidation.AssertValidResponseSchema(result.Content, SchemaValidation.ResponseSchemaType.CheckServiceDetailsById);
        }
    }
}