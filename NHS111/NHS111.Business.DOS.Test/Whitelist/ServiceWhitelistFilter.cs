using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.WhitelistFilter;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Business.DOS.Test.Whitelist
{
    public class ServiceWhitelistFilterTest
    {
        private Mock<Configuration.IConfiguration> _mockConfiguration;
        private Mock<IRestClient> _restClient;
        private readonly string _localServiceIdWhiteListUrl = "http://localhost/whitelist/{0}";

        private static readonly string CheckCapacitySummaryResults = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    }
                },
                {
                    ""idField"": 1419419102,
                    ""nameField"": ""Test Service 2"",
                    ""serviceTypeField"": {
                        ""idField"": 25,
                    }
                },
                {
                    ""idField"": 1419419103,
                    ""nameField"": ""Test Service 3"",
                    ""serviceTypeField"": {
                        ""idField"": 46,
                    }
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
        public async void ServiceWhitelistFilter_RemoveServiceNotInWhitelist()
        {
            const string postcode = "SO302UN";
            string whitelist = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<List<int>>(It.Is<RestRequest>(req => req.Resource.Equals(whitelist)))).Returns(() => StartedTask((IRestResponse<List<int>>)new RestResponse<List<int>>() { ResponseStatus = ResponseStatus.Completed, Data = new List<int> { 1419419101, 1419419102 } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new ServiceWhitelistFilter(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Filter(results, postcode);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1419419101, result[0].Id);
            Assert.AreEqual(1419419102, result[1].Id);
        }

        [Test]
        public async void ServiceWhitelistFilter_EmptyWhitelistReturnsAllResults()
        {
            const string postcode = "SO302UN";
            string whitelist = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<List<int>>(It.Is<RestRequest>(req => req.Resource.Equals(whitelist)))).Returns(() => StartedTask((IRestResponse<List<int>>)new RestResponse<List<int>>() { ResponseStatus = ResponseStatus.Completed, Data = new List<int> {  } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new ServiceWhitelistFilter(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Filter(results, postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1419419101, result[0].Id);
            Assert.AreEqual(1419419102, result[1].Id);
            Assert.AreEqual(1419419103, result[2].Id);
        }


        [Test]
        public async void ServiceWhitelistFilter_WhitelistContainsAllServices()
        {
            const string postcode = "SO302UN";
            string whitelist = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<List<int>>(It.Is<RestRequest>(req => req.Resource.Equals(whitelist)))).Returns(() => StartedTask((IRestResponse<List<int>>)new RestResponse<List<int>>() { ResponseStatus = ResponseStatus.Completed, Data = new List<int> { 1419419101, 1419419102, 1419419103 } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new ServiceWhitelistFilter(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Filter(results, postcode);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1419419101, result[0].Id);
            Assert.AreEqual(1419419102, result[1].Id);
            Assert.AreEqual(1419419103, result[2].Id);
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
