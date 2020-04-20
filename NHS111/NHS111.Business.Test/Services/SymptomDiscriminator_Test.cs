using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NHS111.Business.Services;
using NHS111.Models.Models.Business.Caching;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Cache;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Business.Test.Services
{
    public class SymptomDiscriminator_Test
    {
        private Mock<Configuration.IConfiguration> _configuration;
        private Mock<IRestClient> _restClient;
        private Mock<ICacheManager<string, string>> _cacheManagerMock;
        private ICacheStore _cacheStoreMock;

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<Configuration.IConfiguration>();
            _restClient = new Mock<IRestClient>();
            _cacheManagerMock = new Mock<ICacheManager<string, string>>();
            _cacheStoreMock = new RedisCacheStore(_cacheManagerMock.Object, true);

        }

        [Test]
        public async void GetSymptomDisciminator_should_return_a_symptomDiscrimnator_by_id()
        {
            //Arrange
            var url = "http://mytest.com/";
            var id = 1;
            var symptomDiscriminator = new SymptomDiscriminator(){Id = id, Description = "A description", ReasoningText = "Some reasoning"};
            var response = new Mock<IRestResponse<SymptomDiscriminator>>();
            response.Setup(x => x.IsSuccessful).Returns(true);
            response.Setup(x => x.Data).Returns(symptomDiscriminator);

            _configuration.Setup(x => x.GetDomainApiSymptomDisciminatorUrl(id.ToString())).Returns(url);
            _restClient.Setup(x => x.ExecuteTaskAsync<SymptomDiscriminator>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new SymptomDisciminatorService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetSymptomDisciminator(id.ToString());

            //Assert 
            _configuration.Verify(x => x.GetDomainApiSymptomDisciminatorUrl(id.ToString()), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<SymptomDiscriminator>(It.IsAny<IRestRequest>()), Times.Once);

            Assert.AreEqual(result.Id, id);
        }

        [Test]
        public async void GetSymptomDisciminator_should_return_a_symptomDiscrimnator_by_id_before_adding_to_cache()
        {
            //Arrange
            var id = 1;
            var symptomDiscriminator = new SymptomDiscriminator() { Id = id, Description = "A description", ReasoningText = "Some reasoning" };
            var response = new Mock<IRestResponse<SymptomDiscriminator>>();
            response.Setup(x => x.IsSuccessful).Returns(true);
            response.Setup(x => x.Data).Returns(symptomDiscriminator);
            var expectedCacheKey = new SymptomDiscriminatorCacheKey(id.ToString());

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);
            _restClient.Setup(x => x.ExecuteTaskAsync<SymptomDiscriminator>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new SymptomDisciminatorService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetSymptomDisciminator(id.ToString());

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _configuration.Verify(x => x.GetDomainApiSymptomDisciminatorUrl(id.ToString()), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<SymptomDiscriminator>(It.IsAny<IRestRequest>()), Times.Once);

            Assert.AreEqual(result.Id, id);
        }
    }
}
