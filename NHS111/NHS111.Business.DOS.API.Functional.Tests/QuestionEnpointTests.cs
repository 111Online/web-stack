

namespace NHS111.Business.DOS.API.Functional.Tests
{
    using System.ComponentModel;
    using System.Net.Http;
    using System.Text;
    using NHS111.Utils.Helpers;
    using NUnit.Framework;
    using Newtonsoft.Json.Linq;

    [TestFixture]
    public class QuestionEnpointTests
    {
        private string _domainApiDomain =
            "https://microsoft-apiapp9783ffa9d6244e6590fec540-integration.azurewebsites.net/";


        private RestfulHelper _restfulHelper = new RestfulHelper();

        /// <summary>
        /// Example test method for a HTTP POST
        /// </summary>
        [Test]
        public async void TestCheckDoSBusinessCapacitySumary()
        {
            var getNextQuestionEndpoint = "DOSapi/CheckCapacitySummary";
            var result = await _restfulHelper.PostAsync(_domainApiDomain + getNextQuestionEndpoint, CreateHTTPRequest("{\"ServiceVersion\":\"1.3\",\"UserInfo\":{\"Username\":\"digital111_ws\",\"Password\":\"Valtech111\"},\"c\":{\"Postcode\":\"HP21 8AL\"}}"));

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

        public static HttpRequestMessage CreateHTTPRequest(string requestContent)
        {
            return new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };
        }

        [Test]
        public async void TestCheckDosBusinessServiceDetailsById()
        {
            var getNextQuestionEndpoint = "DOSapi/ServiceDetailsById";
            var result = await _restfulHelper.PostAsync(_domainApiDomain + getNextQuestionEndpoint, CreateHTTPRequest("{\"ServiceVersion\":\"1.3\",\"UserInfo\":{\"Username\":\"digital111_ws\",\"Password\":\"Valtech111\"},\"serviceId\":1315835856}"));

            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.IsTrue(result.IsSuccessStatusCode);
            AssertValidResponseSchema(resultContent, ResponseSchemaType.CheckServiceDetailsById);
        }


        public enum ResponseSchemaType
        {

            CheckServiceDetailsById
        }

        private static void AssertValidResponseSchema(string result, ResponseSchemaType schemaType)
        {
            switch (schemaType)
            {

                case ResponseSchemaType.CheckServiceDetailsById:
                    AssertValidCheckServiceDetailsByIdResponseSchema(result);
                    break;
                default:
                    throw new InvalidEnumArgumentException("ResponseSchemaType of " + schemaType.ToString() +
                                                       "is unsupported");
            }
        }


        private static void AssertValidCheckServiceDetailsByIdResponseSchema(string result)
        {
            Assert.IsTrue(result.Contains("\"tagField"));
            Assert.IsTrue(result.Contains("\"nameField"));
            Assert.IsTrue(result.Contains("\"valueField"));
            Assert.IsTrue(result.Contains("\"orderField"));
            Assert.IsTrue(result.Contains("\"PropertyChanged"));
        }
    }
}
