using System.Configuration;
using Newtonsoft.Json.Linq;
using NHS111.Functional.Tests.Tools;
using NHS111.Utils.Helpers;
using NUnit.Framework;

namespace NHS111.DOS.Domain.API.Functional.Tests
{
    [TestFixture]
    public class DomainDOSApiTests
    {
        private static string DomainDOSApiCheckCapacitySummaryUrl
        {
            get { return ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"] + ConfigurationManager.AppSettings["DomainDOSApiCheckCapacitySummaryUrl"]; }
        }

        private static string DomainDOSApiServiceDetailsByIdUrl
        {
            get { return ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"] + ConfigurationManager.AppSettings["DomainDOSApiServiceDetailsByIdUrl"]; }
        }

        private static string DomainDOSApiServicesByClinicalTermUrl
        {
            get { return ConfigurationManager.AppSettings["DomainDOSApiBaseUrl"] + ConfigurationManager.AppSettings["DomainDOSApiServicesByClinicalTermUrl"]; }
        }
        
        private static string DOSApiUsername
        {
            get { return ConfigurationManager.AppSettings["dos_credential_user"]; }
        }

        private static string DOSApiPassword
        {
            get { return ConfigurationManager.AppSettings["dos_credential_password"]; }
        }

        private RestfulHelper _restfulHelper = new RestfulHelper();

        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestCheckCapacitySumary()
        {
            var result = await _restfulHelper.PostAsync(DomainDOSApiCheckCapacitySummaryUrl, RequestFormatting.CreateHTTPRequest("{\"ServiceVersion\":\"1.3\",\"UserInfo\":{\"Username\":\"" + DOSApiUsername + "\",\"Password\":\"" + DOSApiPassword + "\"},\"c\":{\"Postcode\":\"HP21 8AL\"}}",string.Empty));

            var resultContent = await result.Content.ReadAsStringAsync();
            dynamic jsonResult = Newtonsoft.Json.Linq.JObject.Parse(resultContent);
            JArray summaryResult = jsonResult.CheckCapacitySummaryResult;
            dynamic firstService = summaryResult[0];

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
        public async void TestCheckServiceDetailsById()
        {
            var result = await _restfulHelper.PostAsync(DomainDOSApiServiceDetailsByIdUrl, RequestFormatting.CreateHTTPRequest("{\"ServiceVersion\":\"1.3\",\"UserInfo\":{\"Username\":\"" + DOSApiUsername + "\",\"Password\":\"" + DOSApiPassword + "\"},\"serviceId\":1315835856}", string.Empty));

            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.IsTrue(result.IsSuccessStatusCode);
            SchemaValidation.AssertValidResponseSchema(resultContent, SchemaValidation.ResponseSchemaType.CheckServiceDetailsById);
        }

        [Test]
        public async void TestServicesByClinicalTerm()
        {
            var caseId = "0";
            var postCode = "LS17 7NZ";
            var searchDistance = "36";
            var gpPracticeId = "0";
            var age = "1";
            var gender = "F";
            var dispo = "Dx02";
            var sg = "1108";
            var sd = "4009";
            var numberPerType = "1";

            var result = await _restfulHelper.PostAsync(DomainDOSApiServicesByClinicalTermUrl, RequestFormatting.CreateHTTPRequest(string.Format("{{\"caseId\":\"{0}\",\"postcode\":\"{1}\",\"searchDistance\":\"{2}\",\"gpPracticeId\":\"{3}\",\"age\":\"{4}\",\"gender\":\"{5}\",\"disposition\":\"{6}\",\"symptomGroupDiscriminatorCombos\":\"{7}={8}\",\"numberPerType\":\"{9}\" }}", caseId, postCode, searchDistance, gpPracticeId, age, gender, dispo, sg, sd, numberPerType),string.Empty));

            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.IsTrue(resultContent.Contains("Leeds"));
        }
    }
}
