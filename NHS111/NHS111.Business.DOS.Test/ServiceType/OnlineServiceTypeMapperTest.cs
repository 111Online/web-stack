using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.Service;
using NHS111.Models.Models.Web.CCG;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Business.DOS.Test.ServiceType
{
    public class OnlineServiceTypeMapperTest
    {
        private Mock<Configuration.IConfiguration> _mockConfiguration;
        private Mock<IRestClient> _restClient;
        private readonly string _localServiceIdWhiteListUrl = "http://localhost/whitelist/{0}";
        private readonly string _phoneServiceReferralText = "You must telephone this service before attending";
        private readonly string _goToServiceReferralText = "You can go straight to this service. You do not need to telephone beforehand";

        private static readonly string CheckCapacitySummaryResultsNoReferralText = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralText"": """"
                },
                {
                    ""idField"": 1419419102,
                    ""nameField"": ""Test Service 2"",
                    ""serviceTypeField"": {
                        ""idField"": 25,
                    },
                    ""referralText"": """"
                },
                {
                    ""idField"": 1419419103,
                    ""nameField"": ""Test Service 3"",
                    ""serviceTypeField"": {
                        ""idField"": 46,
                    },
                    ""referralText"": """"
                },
            ]}";

        private static readonly string CheckCapacitySummaryResultsGoToPhoneGoToReferralText = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralText"": ""You can go straight to this service. You do not need to telephone beforehand""
                },
                {
                    ""idField"": 1419419102,
                    ""nameField"": ""Test Service 2"",
                    ""serviceTypeField"": {
                        ""idField"": 25,
                    },
                    ""referralText"": ""You must telephone this service before attending""
                },
                {
                    ""idField"": 1419419103,
                    ""nameField"": ""Test Service 3"",
                    ""serviceTypeField"": {
                        ""idField"": 46,
                    },
                    ""referralText"": ""You can go straight to this service. You do not need to telephone beforehand""
                },
            ]}";

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<Configuration.IConfiguration>();
            _restClient = new Mock<IRestClient>();

            _mockConfiguration.Setup(c => c.CCGApiGetCCGByPostcode).Returns(_localServiceIdWhiteListUrl);
        }

        [Test]
        public async void OnlineServiceTypeMapper_CallbackEnabledForWhitelistedServiceId()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ItkServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419102" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsNoReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.Unknown, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.Callback, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.Unknown, result[2].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_EmptyWhitelist()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ItkServiceIdWhitelist = new ServiceListModel {  } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsGoToPhoneGoToReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[2].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_NullWhitelist()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsGoToPhoneGoToReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[2].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_CallbackNotEnabledForNonWhitelistedServiceId()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ItkServiceIdWhitelist = new ServiceListModel { "123", "456", "789" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsGoToPhoneGoToReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[2].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_EmptyDOSResult()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ItkServiceIdWhitelist = new ServiceListModel { "1419419101", "1419419102", "1419419103" } } }));

            const string emptyCheckCapacityResults = @"{
            ""CheckCapacitySummaryResult"": [
            ]}";
            var jObj = (JObject)JsonConvert.DeserializeObject(emptyCheckCapacityResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, postcode);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async void OnlineServiceTypeMapper_CCGServiceServerError()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.InternalServerError, ResponseStatus = ResponseStatus.Completed }));

            const string emptyCheckCapacityResults = @"{
            ""CheckCapacitySummaryResult"": [
            ]}";
            var jObj = (JObject)JsonConvert.DeserializeObject(emptyCheckCapacityResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            try
            {
                var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
                var result = await sut.Map(results, postcode);
                Assert.Fail();
            }
            catch (HttpException e)
            {
                Assert.Pass();
            }
        }

        private static Task<T> StartedTask<T>(T taskResult)
        {
            return Task<T>.Factory.StartNew(() => taskResult);
        }

        private static JObject GetJObjectFromResponse(HttpResponseMessage response)
        {
            var val = response.Content.ReadAsStringAsync().Result;
            return (JObject)JsonConvert.DeserializeObject(val);
        }
    }
}
