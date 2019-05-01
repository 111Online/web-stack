using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.Service;
using NHS111.Models.Models.Web.CCG;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Business.DOS.Test.Service
{
    public class SearchDistanceServiceTests
    {
        private Mock<Configuration.IConfiguration> _mockConfiguration;
        private Mock<IRestClient> _restClient;
        private readonly string _localCCGServiceUrl = "http://localhost/api/ccg/{0}";

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<Configuration.IConfiguration>();
            _restClient = new Mock<IRestClient>();

            _mockConfiguration.Setup(c => c.CCGApiGetCCGByPostcode).Returns(_localCCGServiceUrl);
            _mockConfiguration.Setup(c => c.DoSSearchDistance).Returns(60);
        }

        [Test]
        public async void null_response_returns_default_from_config()
        {
            const string postcode = "SO302UN";
            var ccgUrl = string.Format(_localCCGServiceUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync(It.Is<RestRequest>(req => req.Resource.Equals(ccgUrl)))).Returns(() => StartedTask((IRestResponse)new RestResponse() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Content = null }));

            var sut = new SearchDistanceService(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.GetSearchDistanceByPostcode(postcode);

            Assert.AreEqual(result, 60);
        }

        [Test]
        public async void empty_response_returns_default_from_config()
        {
            const string postcode = "SO302UN";
            var ccgUrl = string.Format(_localCCGServiceUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync(It.Is<RestRequest>(req => req.Resource.Equals(ccgUrl)))).Returns(() => StartedTask((IRestResponse)new RestResponse() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Content = JsonConvert.SerializeObject(new CCGModel { SearchDistance = string.Empty }) }));

            var sut = new SearchDistanceService(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.GetSearchDistanceByPostcode(postcode);

            Assert.AreEqual(result, 60);
        }

        [Test]
        public async void valid_response_returns_search_distance()
        {
            const string postcode = "SO302UN";
            var ccgUrl = string.Format(_localCCGServiceUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync(It.Is<RestRequest>(req => req.Resource.Equals(ccgUrl)))).Returns(() => StartedTask((IRestResponse)new RestResponse() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Content = JsonConvert.SerializeObject(new CCGModel { SearchDistance = "100" }) }));

            var sut = new SearchDistanceService(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.GetSearchDistanceByPostcode(postcode);

            Assert.AreEqual(result, 100);
        }

        private static Task<T> StartedTask<T>(T taskResult)
        {
            return Task<T>.Factory.StartNew(() => taskResult);
        }
    }
}
