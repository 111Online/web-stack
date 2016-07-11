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
        private RestfulHelper _restfulHelper = new RestfulHelper();

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
        
        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestCheckDoSBusinessCapacitySumary()
        {
            var result =
                await
                    _restfulHelper.PostAsync(BusinessDosCheckCapacitySummaryUrl,
                        RequestFormatting.CreateHTTPRequest(
                            "{\"ServiceVersion\":\"1.3\",\"UserInfo\":{\"Username\":\"" + DOSApiUsername + "\",\"Password\":\"" + DOSApiPassword + "\"},\"c\":{\"Postcode\":\"HP21 8AL\"}}", string.Empty));

            var resultContent = await result.Content.ReadAsStringAsync();
            dynamic jsonResult = Newtonsoft.Json.Linq.JObject.Parse(resultContent);
            JArray summaryResult = jsonResult.CheckCapacitySummaryResult;
            dynamic firstService = summaryResult[0];
            var serviceTypeField = firstService.serviceTypeField;

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
            Assert.IsNotNull(serviceTypeField.PropertyChanged);

            dynamic rootParentField = response.rootParentField;
            Assert.IsNotNull(rootParentField.idField);
            Assert.IsNotNull(rootParentField.nameField);
            Assert.IsNotNull(rootParentField.PropertyChanged);

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
            Assert.IsNotNull(response.obsoleteField);
            Assert.IsNotNull(response.updateTimeField);
            Assert.IsNotNull(response.openAllHoursField);
            Assert.IsNotNull(response.rotaSessionsField);
            Assert.IsNotNull(response.serviceTypeField);
            Assert.IsNotNull(response.odsCodeField);
            Assert.IsNotNull(response.rootParentField);
            Assert.IsNotNull(response.PropertyChanged);
        }

        [Test]
        public async void TestCheckDosBusinessServiceDetailsById()
        {
            var result =
                await
                    _restfulHelper.PostAsync(BusinessDosServiceDetailsByIdUrl,
                        RequestFormatting.CreateHTTPRequest(
                            "{\"ServiceVersion\":\"1.3\",\"UserInfo\":{\"Username\":\"" + DOSApiUsername + "\",\"Password\":\"" + DOSApiPassword + "\"},\"serviceId\":1315835856}", string.Empty));

            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.IsTrue(result.IsSuccessStatusCode);
            SchemaValidation.AssertValidResponseSchema(resultContent, SchemaValidation.ResponseSchemaType.CheckServiceDetailsById);
        }
    }
}
