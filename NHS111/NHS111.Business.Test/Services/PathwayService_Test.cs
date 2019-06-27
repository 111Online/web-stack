using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Business.Services;
using NHS111.Utils.Helpers;
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using RestSharp;

namespace NHS111.Business.Test.Services
{
    [TestFixture]
    public class PathwayService_Test
    {

        private Mock<Configuration.IConfiguration> _configuration;
        private Mock<IRestClient> _restClient;

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<Configuration.IConfiguration>();
            _restClient = new Mock<IRestClient>();
        }

        [Test]
        public async void should_return_a_collection_of_pathways()
        {
            //Arrange
            var url = "http://mytest.com/";
            var unique = true;
            var pathways = new[] {new Pathway {Title = "pathway1"}, new Pathway {Title = "pathway2"},};

            var response = new Mock<IRestResponse<IEnumerable<Pathway>>>();
            response.Setup(_ => _.Data).Returns(pathways);

            _configuration.Setup(x => x.GetDomainApiPathwaysUrl(unique, false)).Returns(url);
            _restClient.Setup(x => x.ExecuteTaskAsync<IEnumerable<Pathway>>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object);

            //Act
            var result = await sut.GetPathways(unique, false);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiPathwaysUrl(unique, false), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<IEnumerable<Pathway>>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result.First().Title, "pathway1");
        }

        [Test]
        public async void should_return_a_single_pathway_by_id()
        {
            //Arrange

            var url = "http://mytest.com/";
            var id = "PW123";
            var pathway = new Pathway { Title = "pathway1" };

            var response = new Mock<IRestResponse<Pathway>>();
            response.Setup(_ => _.Data).Returns(pathway);

            _configuration.Setup(x => x.GetDomainApiPathwayUrl(id)).Returns(url);
            _restClient.Setup(x => x.ExecuteTaskAsync<Pathway>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object);

            //Act
            var result = await sut.GetPathway(id);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiPathwayUrl(id), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<Pathway>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.Title, "pathway1");
        }

        

        [Test]
        public async void should_return_a_single_pathway_metadata_by_id()
        {
            //Arrange

            var url = "http://mytest.com/";
            var id = "PW123";
            var pathway = new PathwayMetaData() { DigitalTitle = "pathway1" };

            var response = new Mock<IRestResponse<PathwayMetaData>>();
            response.Setup(_ => _.Data).Returns(pathway);

            _configuration.Setup(x => x.GetDomainApiPathwayMetadataUrl(id)).Returns(url);
            _restClient.Setup(x => x.ExecuteTaskAsync<PathwayMetaData>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object);

            //Act
            var result = await sut.GetPathwayMetaData(id);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiPathwayMetadataUrl(id), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<PathwayMetaData>(It.IsAny<IRestRequest>()), Times.Once);
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
            _restClient.Setup(x => x.ExecuteTaskAsync<Pathway>(It.IsAny<IRestRequest>()))
                .ReturnsAsync(response.Object);

            var sut = new PathwayService(_configuration.Object, _restClient.Object);

            //Act
            var result = await sut.GetIdentifiedPathway(pathwayNo, gender, age);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiIdentifiedPathwayUrl(pathwayNo, gender, age), Times.Once);
            _restClient.Verify(x => x.ExecuteTaskAsync<Pathway>(It.IsAny<IRestRequest>()), Times.Once);
            Assert.AreEqual(result.Title, "identified pathway");
        }
    }
}
