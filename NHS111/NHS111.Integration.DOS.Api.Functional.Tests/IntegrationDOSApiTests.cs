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
            get { return ConfigurationManager.AppSettings["DOSIntegrationBaseUrl"] + ConfigurationManager.AppSettings["DOSIntegrationCheckCapacitySummaryUrl"]; }
        }

        private static string DOSIntegrationServiceDetailsByIdUrl
        {
            get { return ConfigurationManager.AppSettings["DOSIntegrationBaseUrl"] + ConfigurationManager.AppSettings["DOSIntegrationServiceDetailsByIdUrl"]; }
        }

        private RestfulHelper _restfulHelper = new RestfulHelper();

        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestCheckDosIntegrationCapacitySumary()
        {
            var result = await _restfulHelper.PostAsync(DOSIntegrationCheckCapacitySummaryUrl, RequestFormatting.CreateHTTPRequest("{\"ServiceVersion\":\"1.3\",\"UserInfo\":{\"Username\":\"" + DOSApiUsername + "\",\"Password\":\"" + DOSApiPassword + "\"},\"c\":{\"Postcode\":\"HP21 8AL\"}}", string.Empty));

            var resultContent = await result.Content.ReadAsStringAsync();
            dynamic jsonResult = Newtonsoft.Json.Linq.JObject.Parse(resultContent);
            JArray summaryResult = jsonResult.CheckCapacitySummaryResult;
            dynamic firstService = summaryResult[0];
            dynamic serviceTypeField = firstService.serviceTypeField;

            AssertResponse(firstService);
            //Assert.IsNotNull(serviceTypeField.idField);
            //Assert.AreEqual("40", (string)serviceTypeField.idField);
            Assert.IsTrue(result.IsSuccessStatusCode);

        }

        private void AssertResponse(dynamic response)
        {
            dynamic serviceTypeField = response.serviceTypeField;
            Assert.IsNotNull(serviceTypeField.idField);
            Assert.IsNotNull(serviceTypeField.nameField);

            Assert.IsNotNull(response.idField);
            Assert.IsNotNull(response.capacityField);
            Assert.IsNotNull(response.nameField);
            Assert.IsNotNull(response.contactDetailsField);
            Assert.IsNotNull(response.addressField);
            Assert.IsNotNull(response.postcodeField);
            Assert.IsNotNull(response.northingsField);
            Assert.IsNotNull(response.northingsFieldSpecified);
            Assert.IsNotNull(response.eastingsField);
            Assert.IsNotNull(response.eastingsFieldSpecified);
            Assert.IsNotNull(response.urlField);
            Assert.IsNotNull(response.notesField);

            Assert.IsNotNull(response.openAllHoursField);
            Assert.IsNotNull(response.rotaSessionsField);
            Assert.IsNotNull(response.serviceTypeField);
            Assert.IsNotNull(response.odsCodeField);

        }

        [Test]
        public async void TestCheckDosIntegrationServiceDetailsById()
        {
            var result = await _restfulHelper.PostAsync(DOSIntegrationServiceDetailsByIdUrl, RequestFormatting.CreateHTTPRequest("{\"ServiceVersion\":\"1.3\",\"UserInfo\":{\"Username\":\"" + DOSApiUsername + "\",\"Password\":\"" + DOSApiPassword + "\"},\"serviceId\":1315835856}", string.Empty));

            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.IsTrue(result.IsSuccessStatusCode);
            SchemaValidation.AssertValidResponseSchema(resultContent, SchemaValidation.ResponseSchemaType.CheckServiceDetailsById);
        }
    }
}