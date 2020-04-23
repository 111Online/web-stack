using System.Runtime.InteropServices;
using NHS111.Business.Services;
using NHS111.Utils.Helpers;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Business.Caching;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Cache;
using RestSharp;

namespace NHS111.Business.Test.Services
{
    [TestFixture]
    public class CareAdviceService_Test
    {
        private Mock<Configuration.IConfiguration> _configuration;
        private Mock<IRestClient> _restClient;
        private Mock<ICacheManager<string, string>> _cacheManagerMock;
        private ICacheStore _cacheStoreMock;
        private Mock<IRestResponse<IEnumerable<CareAdvice>>> _mockCareAdviceRestResponse;

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<Configuration.IConfiguration>();
            _restClient = new Mock<IRestClient>();
            _mockCareAdviceRestResponse = new Mock<IRestResponse<IEnumerable<CareAdvice>>>();
            _cacheManagerMock = new Mock<ICacheManager<string, string>>();
            _cacheStoreMock = new RedisCacheStore(_cacheManagerMock.Object, true);

            _mockCareAdviceRestResponse.SetupGet(x => x.IsSuccessful).Returns(true);
           

        }

        [Test]
        public async void GetCareAdviceByKeywords_should_return_care_advice()
        {
            //Arrange
            var dxCode = "Dx123";
            var pathwayNo = "PW1234";
            string ageCategory = "Adult";
            string gender = "Male";
            string keywords = "one,two,three";
            InitialiseMockRestResponse(MOCK_CAREADVICE);

            _restClient.Setup(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(_mockCareAdviceRestResponse.Object);


            var sut = new CareAdviceService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetCareAdvice(ageCategory, gender, keywords, dxCode);
            //Assert
            _configuration.Verify(x => x.GetDomainApiCareAdviceUrl(dxCode, ageCategory, gender), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result, Is.EqualTo(_mockCareAdviceRestResponse.Object.Data));

        }

        [Test]
        public async void GetCareAdviceByKeywords_should_not_add_care_advice_to_cache_for_null_restclient_response()
        {
            //Arrange
            var dxCode = "Dx123";
            var pathwayNo = "PW1234";
            string ageCategory = "Adult";
            string gender = "Male";
            string keywords = "one,two,three";
            var expectedCacheKey = new CareAdviceCacheKey(ageCategory,gender, keywords, dxCode);
            InitialiseMockRestResponse(null);
            _restClient.Setup(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(_mockCareAdviceRestResponse.Object);

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new CareAdviceService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetCareAdvice(ageCategory, gender, keywords, dxCode);
            //Assert
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Never);
            _restClient.Verify(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result, Is.EqualTo(_mockCareAdviceRestResponse.Object.Data));
        }

        [Test]
        public async void GetCareAdviceByKeywords_should_not_call_restClient_if_CareAdvice_present_in_cache()
        {
            //Arrange
            var dxCode = "Dx123";
            var pathwayNo = "PW1234";
            string ageCategory = "Adult";
            string gender = "Male";
            string keywords = "one,two,three";
            var expectedCacheKey = new CareAdviceCacheKey(ageCategory, gender, keywords, dxCode);
            InitialiseMockRestResponse(MOCK_CAREADVICE);
            _restClient.Setup(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(_mockCareAdviceRestResponse.Object);

            _cacheManagerMock.Setup(x => x.Read(expectedCacheKey.CacheKey)).ReturnsAsync(JsonConvert.SerializeObject(MOCK_CAREADVICE));

            var sut = new CareAdviceService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetCareAdvice(ageCategory, gender, keywords, dxCode);
            //Assert
            _cacheManagerMock.Verify(x => x.Read(expectedCacheKey.CacheKey), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()), Times.Never);
            Assert.That(result.First().Title, Is.EqualTo(MOCK_CAREADVICE.First().Title));

        }

        [Test]
        public async void GetCareAdviceByKeywords_should_not_add_care_advice_to_cache_for_empty_restclient_response()
        {
            //Arrange
            var dxCode = "Dx123";
            var pathwayNo = "PW1234";
            string ageCategory = "Adult";
            string gender = "Male";
            string keywords = "one,two,three";
            var expectedCacheKey = new CareAdviceCacheKey(ageCategory, gender, keywords, dxCode);
            InitialiseMockRestResponse(new CareAdvice[0]);
            _restClient.Setup(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(_mockCareAdviceRestResponse.Object);

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new CareAdviceService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetCareAdvice(ageCategory, gender, keywords, dxCode);
            //Assert
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Never);
            _restClient.Verify(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result, Is.EqualTo(_mockCareAdviceRestResponse.Object.Data));

        }

        [Test]
        public async void GetCareAdviceByKeywords_should_return_care_advice_befire_adding_to_cache()
        {
            //Arrange
            var dxCode = "Dx123";
            var pathwayNo = "PW1234";
            string ageCategory = "Adult";
            string gender = "Male";
            string keywords = "one,two,three";
            var expectedCacheKey = new CareAdviceCacheKey(ageCategory, gender, keywords, dxCode);
            InitialiseMockRestResponse(MOCK_CAREADVICE);
            _restClient.Setup(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(_mockCareAdviceRestResponse.Object);

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new CareAdviceService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetCareAdvice(ageCategory, gender, keywords, dxCode);
            //Assert
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result, Is.EqualTo(_mockCareAdviceRestResponse.Object.Data));
        }

        [Test]
        public async void GetCareAdviceByMarkers_should_return_care_advice_befire_adding_to_cache()
        {
            //Arrange
            int age = 22;
            string gender = "Male";
            List<string> markers = new List<string>() {"one", "two", "three"};
            var expectedCacheKey = new CareAdviceCacheKey(age, gender, markers);
            InitialiseMockRestResponse(MOCK_CAREADVICE);
            _restClient.Setup(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(_mockCareAdviceRestResponse.Object);

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new CareAdviceService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetCareAdvice(age, gender, markers);
            //Assert
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<IEnumerable<CareAdvice>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.That(result, Is.EqualTo(_mockCareAdviceRestResponse.Object.Data));
        }

        private void InitialiseMockRestResponse(IEnumerable<CareAdvice> value)
        {
            _mockCareAdviceRestResponse.SetupGet(x => x.Data).Returns(value);
        }

        private IEnumerable<CareAdvice> MOCK_CAREADVICE = new List<CareAdvice>()
        {
            new CareAdvice()
            {
                Id = "CAid2", Title = "Some care advice",
                Items = new List<CareAdviceText>()
                    {new CareAdviceText() {Id = "Text1", Text = "Some advice", OrderNo = 1}}
            }
        };

    }

}