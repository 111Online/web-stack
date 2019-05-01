using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.Service;
using NHS111.Models.Models.Web.CCG;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Business.DOS.Test.ServiceType
{
    public class OnlineServiceTypeMapperTest
    {
        private Mock<Configuration.IConfiguration> _mockConfiguration;
        private Mock<IRestClient> _restClient;
        private readonly string _localServiceIdWhiteListUrl = "http://localhost/api/ccg/details/{0}";
        private readonly string _postcode = "SO312IN";

        #region CheckCapacitySummary Result Examples

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
                    ""referralTextField"": """"
                },
            ]}";

        private static readonly string CheckCapacitySummaryResultsGoToPhoneGoToReferralText = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralTextField"": ""You can go straight to this service. You do not need to telephone beforehand""
                },
                {
                    ""idField"": 1419419102,
                    ""nameField"": ""Test Service 2"",
                    ""serviceTypeField"": {
                        ""idField"": 25,
                    },
                    ""referralTextField"": ""You must telephone this service before attending"",
                    ""contactDetailsField"": ""02355 444777""
                },
                {
                    ""idField"": 1419419103,
                    ""nameField"": ""Test Service 3"",
                    ""serviceTypeField"": {
                        ""idField"": 46,
                    },
                    ""referralTextField"": ""You can go straight to this service. You do not need to telephone beforehand""
                },
            ]}";

        private static readonly string CheckCapacitySummaryResultsSingleGoToReferralText = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralTextField"": ""You can go straight to this service. You do not need to telephone beforehand""
                }
            ]}";

        private static readonly string CheckCapacitySummaryResultsSinglePhoneReferralText = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralTextField"": ""You must telephone this service before attending"",
                    ""contactDetailsField"": ""02355 444777""
                }
            ]}";

        private static readonly string CheckCapacitySummaryResultsSinglePhoneWithOtherTextReferralText = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralTextField"": ""Some other text is here. You must telephone this service before attending. Some different text is also here."",
                    ""contactDetailsField"": ""02355 444777""
                }
            ]}";

        private static readonly string CheckCapacitySummaryResultsUnknownReferralText = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralTextField"": ""telephone this service"",
                    ""contactDetailsField"": ""02355 444777""
                }
            ]}";

        private static readonly string CheckCapacitySummaryResultsGoToPhoneGoToReferralTextCasingAndPunctuation = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralTextField"": ""You can G.O. straight to this service. You do not need to telephone beforehand""
                },
                {
                    ""idField"": 1419419102,
                    ""nameField"": ""Test Service 2"",
                    ""serviceTypeField"": {
                        ""idField"": 25,
                    },
                    ""referralTextField"": ""You M.U.s.T, telephone this service before attending"",
                    ""contactDetailsField"": ""02355 444777""
                },
                {
                    ""idField"": 1419419103,
                    ""nameField"": ""Test Service 3"",
                    ""serviceTypeField"": {
                        ""idField"": 46,
                    },
                    ""referralTextField"": ""You can go        straight to this service. You do not need to telephone beforehand""
                },
            ]}";

        private static readonly string CheckCapacitySummaryResultsSinglePhoneReferralTextNoContactDetails = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralTextField"": ""You must telephone this service before attending"",
                    ""contactDetailsField"": """"
                }
            ]}";


        private static readonly string CheckCapacitySummaryResultsReferAndRingReferralText = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralTextField"": ""This service accepts electronic referrals. You should ring before you go there"",
                    ""contactDetailsField"": ""02355 444777""
                }
            ]}";

        private static readonly string CheckCapacitySummaryResultsReferAndRingReferralTextEmptyContactDetails = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    },
                    ""referralTextField"": ""This service accepts electronic referrals. You should ring before you go there"",
                    ""contactDetailsField"": """"
                }
            ]}";
        #endregion

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<Configuration.IConfiguration>();
            _restClient = new Mock<IRestClient>();

            _mockConfiguration.Setup(c => c.CCGApiGetCCGDetailsByPostcode).Returns(_localServiceIdWhiteListUrl);
        }

        [Test]
        public async void OnlineServiceTypeMapper_CallbackEnabledForWhitelistedServiceId()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419102" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsNoReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.Unknown, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.Callback, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.Unknown, result[2].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_EmptyWhitelist()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel {  } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsGoToPhoneGoToReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[2].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_NullWhitelist()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsGoToPhoneGoToReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[2].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_CallbackNotEnabledForNonWhitelistedServiceId()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsGoToPhoneGoToReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[2].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_EmptyDOSResult()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "1419419101", "1419419102", "1419419103" } } }));

            const string emptyCheckCapacityResults = @"{
            ""CheckCapacitySummaryResult"": [
            ]}";
            var jObj = (JObject)JsonConvert.DeserializeObject(emptyCheckCapacityResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async void OnlineServiceTypeMapper_CCGServiceServerError()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
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
                var result = await sut.Map(results, _postcode);
                Assert.Fail();
            }
            catch (HttpException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public async void OnlineServiceTypeMapper_GoToTextCallbackFalse_ReturnsGoTo()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419102" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsSingleGoToReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[0].OnlineDOSServiceType);            
        }

        [Test]
        public async void OnlineDOSServiceType_GoToTextCallbackTrue_ReturnsCallback()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419101" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsSingleGoToReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.Callback, result[0].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineDOSServiceType_PhoneTextCallbackFalse_ReturnsPhone()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419102" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsSinglePhoneReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[0].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineDOSServiceType_PhoneTextCallbackTrue_ReturnsCallback()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419101" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsSinglePhoneReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.Callback, result[0].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineDOSServiceType_UnknownTextCallbackFalse_ReturnsUnknownType()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419102" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsUnknownReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.Unknown, result[0].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineDOSServiceType_UnknownTextCallbackTrue_ReturnsCallback()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419101" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsUnknownReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.Callback, result[0].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineDOSServiceType_PhoneTextInMiddleOfFieldCallbackFalse_ReturnsPhone()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419102" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsSinglePhoneWithOtherTextReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[0].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_PhoneTextAndGoToTextCaseAndPunctuationInsensitive()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsGoToPhoneGoToReferralTextCasingAndPunctuation);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[0].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.PublicPhone, result[1].OnlineDOSServiceType);
            Assert.AreEqual(OnlineDOSServiceType.GoTo, result[2].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineDOSServiceType_PhoneTextCallbackFalseNoContactDetails_ReturnsUnknown()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419102" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsSinglePhoneReferralTextNoContactDetails);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.Unknown, result[0].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_ReferAndRingEnabledForWhitelistedServiceId()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "1419419101" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsReferAndRingReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.ReferRingAndGo, result[0].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_ReferAndRingUnknownForNonWhitelistedServiceId()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "1234" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsReferAndRingReferralText);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.Unknown, result[0].OnlineDOSServiceType);
        }

        [Test]
        public async void OnlineServiceTypeMapper_UnknownReturnedForReferAndRingWithNoContactDetails()
        {
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, _postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ReferralServiceIdWhitelist = new ServiceListModel { "1419419101" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResultsReferAndRingReferralTextEmptyContactDetails);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new OnlineServiceTypeMapper(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Map(results, _postcode);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(OnlineDOSServiceType.Unknown, result[0].OnlineDOSServiceType);
        }


        private static Task<T> StartedTask<T>(T taskResult)
        {
            return Task<T>.Factory.StartNew(() => taskResult);
        }
    }
}
