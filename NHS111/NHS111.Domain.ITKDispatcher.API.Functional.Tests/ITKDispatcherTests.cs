using System.ComponentModel;
using System.Net.Http;
using System.Text;
using NHS111.Utils.Helpers;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;

using NHS111.Models.Models.Web.ITK;

namespace NHS111.Domain.ITKDispatcherTests.API.Functional.Tests
{
    [TestFixture]
    public class ITKDispatcherTests
    {
        private string _domainITKApi =
            "https://nhs111-beta-itkdispatcher.azurewebsites.net/";

        
        private RestfulHelper _restfulHelper = new RestfulHelper();

        //Test to show failure when Name field in service details not sent
        [Test]
        public async void TestInvalidITKMessage_Fails()
        {
            var endpoint = "SendItkMessage";
            var itkDisptatchRequest = ("{\"Authentication\":{\"UserName\":\"admn\",\"Password\":\"admnUat\"},\"PatientDetails\":{\"Forename\":\"TestForename\",\"Surname\":\"TestSurname\",\"Gender\":\"Male\",\"DateOfBirth\":\"11/11/1980\",\"ServiceAddressPostcode\":\"TS196TH\",\"TelephoneNumber\":\"02070 033002\",\"HomeAddress\":{\"PostalCode\":\"TS176TH\",\"StreetAddressLine1\":\"1 Test Lane\"},\"GpPractice\":{\"Name\":\"Test GP\",\"Telephone\":\"02380 666454\",\"ODS\":\"RHYAA\",\"Address\":{\"PostalCode\":\"GP3 6FF\",\"StreetAddressLine1\":\"1 GP Street\"}},\"ServiceDetails\":{\"Id\":\"158960\",\"OdsCode\":\"13524169111352416911\"}}");
            var address = String.Format(_domainITKApi + endpoint);

            System.Net.ServicePointManager.Expect100Continue = false;
            var result =
                await _restfulHelper.PostAsync(address, CreateHTTPRequest(itkDisptatchRequest));

            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.InternalServerError);
            //these check the wrong fields are not returned

        }
                //Test to show failure when invalid authentication is sent
        [Test]
        public async void TestUnauthenticatedITKMessage_Fails()
        {
            var endpoint = "SendItkMessage";
            var itkDisptatchRequest = ("{\"Authentication\":{\"UserName\":\"Test\",\"Password\":\"Test\"},\"PatientDetails\":{\"Forename\":\"TestForename\",\"Surname\":\"TestSurname\",\"Gender\":\"Male\",\"DateOfBirth\":\"11/11/1980\",\"ServiceAddressPostcode\":\"TS196TH\",\"TelephoneNumber\":\"02070 033002\",\"HomeAddress\":{\"PostalCode\":\"TS176TH\",\"StreetAddressLine1\":\"1 Test Lane\"},\"GpPractice\":{\"Name\":\"Test GP\",\"Telephone\":\"02380 666454\",\"ODS\":\"RHYAA\",\"Address\":{\"PostalCode\":\"GP3 6FF\",\"StreetAddressLine1\":\"1 GP Street\"}},\"ServiceDetails\":{\"Id\":\"158960\",\"OdsCode\":\"13524169111352416911\"}}");
            var address = String.Format(_domainITKApi + endpoint);

            System.Net.ServicePointManager.Expect100Continue = false;
            var result =
                await _restfulHelper.PostAsync(address, CreateHTTPRequest(itkDisptatchRequest));

            var resultContent = await result.Content.ReadAsStringAsync();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.InternalServerError);

        }


        public static HttpRequestMessage CreateHTTPRequest(string requestContent)
        {
            return new HttpRequestMessage
            {
                Content = new StringContent(requestContent, Encoding.UTF8, "application/json")
            };
        }

    }
}
