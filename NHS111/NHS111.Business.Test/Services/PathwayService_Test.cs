using Moq;
using NHS111.Business.Services;
using NHS111.Utils.Helpers;
using NUnit.Framework;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Cache;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Business.Caching;
using NHS111.Utils.RestTools;

namespace NHS111.Business.Test.Services
{
    [TestFixture]
    public class PathwayService_Test
    {

        private Mock<Configuration.IConfiguration> _configuration;
        private Mock<ILoggingRestClient> _restClient;
        private Mock<ICacheManager<string, string>> _cacheManagerMock;
        private ICacheStore _cacheStoreMock;

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<Configuration.IConfiguration>();
            _restClient = new Mock<ILoggingRestClient>();
            _cacheManagerMock = new Mock<ICacheManager<string, string>>();
            _cacheStoreMock = new RedisCacheStore(_cacheManagerMock.Object, true);
        }

        [Test]
        public async void should_return_a_collection_of_pathways()
        {
            //Arrange
            var url = "http://mytest.com/";
            var unique = true;
            var pathways = new[] { new Pathway { Title = "pathway1" }, new Pathway { Title = "pathway2" }, };

            var response = new Mock<IRestResponse<IEnumerable<Pathway>>>();
            response.Setup(_ => _.Data).Returns(pathways);

            _configuration.Setup(x => x.GetDomainApiPathwaysUrl(unique, false)).Returns(url);
            _restClient.Setup(x => x.ExecuteAsync<IEnumerable<Pathway>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetPathways(unique, false);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiPathwaysUrl(unique, false), Times.Once);
            _restClient.Verify(x => x.ExecuteAsync<IEnumerable<Pathway>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.First().Title, "pathway1");
        }

        [Test]
        public async void should_return_a_collection_of_pathways_by_age_and_gender()
        {
            //Arrange
            var url = "http://mytest.com/";
            var unique = true;
            var pathways = new[] { new Pathway { Title = "pathway1" }, new Pathway { Title = "pathway2" }, };
            var age = 22;
            string gender = "Male";
            var response = new Mock<IRestResponse<IEnumerable<Pathway>>>();
            response.Setup(_ => _.Data).Returns(pathways);

            _configuration.Setup(x => x.GetDomainApiPathwaysUrl(unique, false)).Returns(url);
            _restClient.Setup(x => x.ExecuteAsync<IEnumerable<Pathway>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetPathways(unique, false, gender, age);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiPathwaysUrl(unique, false, gender, age), Times.Once);
            _restClient.Verify(x => x.ExecuteAsync<IEnumerable<Pathway>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.First().Title, "pathway1");
        }

        [Test]
        public async void GetPathways_should_return_a_collection_of_pathways_by_age_and_gender_before_adding_to_cache()
        {
            //Arrange
            var url = "http://mytest.com/";
            var unique = true;
            var pathways = new[] { new Pathway { Title = "pathway1" }, new Pathway { Title = "pathway2" }, };
            var age = 22;
            string gender = "Male";
            var response = new Mock<IRestResponse<IEnumerable<Pathway>>>();
            response.Setup(_ => _.Data).Returns(pathways);
            var expectedCacheKey = new PathwaysCacheKey(unique, false, gender, age);

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);
            _configuration.Setup(x => x.GetDomainApiPathwaysUrl(unique, false)).Returns(url);
            _restClient.Setup(x => x.ExecuteAsync<IEnumerable<Pathway>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetPathways(unique, false, gender, age);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _configuration.Verify(x => x.GetDomainApiPathwaysUrl(unique, false, gender, age), Times.Once);
            _restClient.Verify(x => x.ExecuteAsync<IEnumerable<Pathway>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.First().Title, "pathway1");
        }

        [Test]
        public async void GetGroupedPathways_should_return_a_collection_of_pathways_by_age_and_gender_before_adding_to_cache()
        {
            //Arrange

            var grouped = true;
            var groupedPathways = new List<GroupedPathways>(){ new GroupedPathways(){Group = "TestGroup", PathwayNumbers = new List<string>() { "pathway1", "pathway2" } }};
     
            string gender = "Male";
            var response = new Mock<IRestResponse<IEnumerable<GroupedPathways>>>();
            response.Setup(_ => _.Data).Returns(groupedPathways);
            var expectedCacheKey = new GroupedPathwaysCacheKey(grouped, true);

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);
            _restClient.Setup(x => x.ExecuteAsync<IEnumerable<GroupedPathways>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetGroupedPathways(grouped, true);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _configuration.Verify(x => x.GetDomainApiGroupedPathwaysUrl(grouped, true), Times.Once);
            _restClient.Verify(x => x.ExecuteAsync<IEnumerable<GroupedPathways>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.First().Group, "TestGroup");
        }


        [Test]
        public async void GetPathway_should_return_a_single_pathway_by_id()
        {
            //Arrange

            var url = "http://mytest.com/";
            var id = "PW123";
            var pathway = new Pathway { Title = "pathway1" };

            var response = new Mock<IRestResponse<Pathway>>();
            response.Setup(_ => _.Data).Returns(pathway);

            _configuration.Setup(x => x.GetDomainApiPathwayUrl(id)).Returns(url);
            _restClient.Setup(x => x.ExecuteAsync<Pathway>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetPathway(id);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiPathwayUrl(id), Times.Once);
            _restClient.Verify(x => x.ExecuteAsync<Pathway>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.Title, "pathway1");
        }
        [Test]
        public async void GetPathway_should_return_a_single_pathway_by_id_before_adding_to_cache()
        {
            //Arrange
            var id = "PW123";
            var pathway = new Pathway { Title = "pathway1", Id = "PW12"};
            var expectedCacheKey = new PathwayCacheKey(id);

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);
            var response = new Mock<IRestResponse<Pathway>>();
            response.Setup(_ => _.Data).Returns(pathway);

            _restClient.Setup(x => x.ExecuteAsync<Pathway>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetPathway(id);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _configuration.Verify(x => x.GetDomainApiPathwayUrl(id), Times.Once);
            _restClient.Verify(x => x.ExecuteAsync<Pathway>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.Title, "pathway1");
        }

        [Test]
        public async void GetPathwayMetaData_should_return_a_single_pathway_metadata_by_id()
        {
            //Arrange

            var url = "http://mytest.com/";
            var id = "PW123";
            var pathway = new PathwayMetaData() { DigitalTitle = "pathway1" };

            var response = new Mock<IRestResponse<PathwayMetaData>>();
            response.Setup(_ => _.Data).Returns(pathway);

            _configuration.Setup(x => x.GetDomainApiPathwayMetadataUrl(id)).Returns(url);
            _restClient.Setup(x => x.ExecuteAsync<PathwayMetaData>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetPathwayMetaData(id);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiPathwayMetadataUrl(id), Times.Once);
            _restClient.Verify(x => x.ExecuteAsync<PathwayMetaData>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.DigitalTitle, "pathway1");
        }

        [Test]
        public async void GetPathwayMetaData_should_return_a_single_pathway_metadata_by_id_before_adding_to_cache()
        {
            //Arrange
            var id = "PW123";
            var pathway = new PathwayMetaData() { DigitalTitle = "pathway1" };
            var expectedCacheKey = new PathwayMetaDataCacheKey(id);

            
            var response = new Mock<IRestResponse<PathwayMetaData>>();
            response.Setup(_ => _.Data).Returns(pathway);

            _cacheManagerMock.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(string.Empty);
            _restClient.Setup(x => x.ExecuteAsync<PathwayMetaData>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetPathwayMetaData(id);

            //Assert 
            _cacheManagerMock.Verify(x => x.Set(expectedCacheKey.CacheKey, It.IsAny<string>()), Times.Once);
            _configuration.Verify(x => x.GetDomainApiPathwayMetadataUrl(id), Times.Once);
            _restClient.Verify(x => x.ExecuteAsync<PathwayMetaData>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.DigitalTitle, "pathway1");
        }

        [Test]
        public async void should_return_an_identified_pathway()
        {
            //Arrange

            var url = "http://mytest.com/";

            var pathwayNo = "PW755";
            var gender = "Male";
            var age = 35;
            var pathway = new Pathway { Title = "identified pathway" };

            var response = new Mock<IRestResponse<Pathway>>();
            response.Setup(_ => _.Data).Returns(pathway);

            _configuration.Setup(x => x.GetDomainApiIdentifiedPathwayUrl(pathwayNo, gender, age)).Returns(url);
            _restClient.Setup(x => x.ExecuteAsync<Pathway>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object, _cacheStoreMock);

            //Act
            var result = await sut.GetIdentifiedPathway(pathwayNo, gender, age);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiIdentifiedPathwayUrl(pathwayNo, gender, age), Times.Once);
            _restClient.Verify(x => x.ExecuteAsync<Pathway>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.Title, "identified pathway");
        }
    }

}
