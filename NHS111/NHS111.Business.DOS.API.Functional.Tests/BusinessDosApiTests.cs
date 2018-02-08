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
                            "{\"PostCode\":\"HP21 8AL\", \"Age\":22, \"Gender\":\"M\"}", string.Empty));

            var resultContent = await result.Content.ReadAsStringAsync();
            dynamic jsonResult = Newtonsoft.Json.Linq.JObject.Parse(resultContent);
            JArray summaryResult = jsonResult.CheckCapacitySummaryResult;
            dynamic firstService = summaryResult[0];
            var serviceTypeField = firstService.ServiceTypeField;

            AssertResponse(firstService);
            //Assert.IsNotNull(serviceTypeField.idField);
            //Assert.AreEqual("40", (string)serviceTypeField.idField);
            Assert.IsTrue(result.IsSuccessStatusCode);
        }

        private void AssertResponse(dynamic response)
        {
            dynamic serviceTypeField = response.ServiceTypeField;
            Assert.IsNotNull(serviceTypeField.idField);
            Assert.IsNotNull(serviceTypeField.nameField);

            Assert.IsNotNull(response.IdField);
            Assert.IsNotNull(response.CapacityField);
            Assert.IsNotNull(response.NameField);
            Assert.IsNotNull(response.ContactDetailsField);
            Assert.IsNotNull(response.AddressField);
            Assert.IsNotNull(response.PostcodeField);
            Assert.IsNotNull(response.NorthingsField);
            Assert.IsNotNull(response.NorthingsSpecifiedField);
            Assert.IsNotNull(response.EastingsField);
            Assert.IsNotNull(response.EastingsSpecifiedField);
            Assert.IsNotNull(response.UrlField);
            Assert.IsNotNull(response.NotesField);
            Assert.IsNotNull(response.ObsoleteField);
            Assert.IsNotNull(response.UpdateTimeField);
            Assert.IsNotNull(response.OpenAllHoursField);
            Assert.IsNotNull(response.RotaSessionsField);
            Assert.IsNotNull(response.ServiceTypeField);
            Assert.IsNotNull(response.OdsCodeField);
            Assert.IsNotNull(response.RootParentField);
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
