using System;
using System.Linq;
using NHS111.Business.Services;
using NHS111.Utils.Helpers;
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;

namespace NHS111.Business.Test.Services
{
    [TestFixture]
    public class PathwayService_Test
    {

        private Mock<Configuration.IConfiguration> _configuration;
        private Mock<IRestfulHelper> _restfulHelper;

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<Configuration.IConfiguration>();
            _restfulHelper = new Mock<IRestfulHelper>();
        }

        [Test]
        public async void should_return_a_collection_of_pathways()
        {
            //Arrange
            var url = "http://mytest.com/";
            var unique = true;
            var resultString = "pathway1, pathway2";

            _configuration.Setup(x => x.GetDomainApiPathwaysUrl(unique, false)).Returns(url);
            _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

            var sut = new PathwayService(_configuration.Object, _restfulHelper.Object);

            //Act
            var result = await sut.GetPathways(unique, false);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiPathwaysUrl(unique, false), Times.Once);
            _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);
            Assert.That(result.Split(new string[] { "," }, StringSplitOptions.None).Count(), Is.EqualTo(2));
            Assert.That(result, Is.EqualTo(resultString));
        }

        [Test]
        public async void should_return_a_single_pathway_by_id()
        {
            //Arrange

            var url = "http://mytest.com/";
            var id = "PW123";
            var resultString = "pathway1";

            _configuration.Setup(x => x.GetDomainApiPathwayUrl(id)).Returns(url);
            _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

            var sut = new PathwayService(_configuration.Object, _restfulHelper.Object);

            //Act
            var result = await sut.GetPathway(id);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiPathwayUrl(id), Times.Once);
            _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);
            Assert.That(result.Split(new string[] { "},{" }, StringSplitOptions.None).Count(), Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(resultString));
        }

        [Test]
        public async void should_return_an_identified_pathway()
        {
            //Arrange

            var url = "http://mytest.com/";

            var pathwayNo = "PW755";
            var gender = "Male";
            var age = 35;

            var resultString = "identified pathway";

            _configuration.Setup(x => x.GetDomainApiIdentifiedPathwayUrl(pathwayNo, gender, age)).Returns(url);
            _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

            var sut = new PathwayService(_configuration.Object, _restfulHelper.Object);

            //Act
            var result = await sut.GetIdentifiedPathway(pathwayNo, gender, age);

            //Assert 
            _configuration.Verify(x => x.GetDomainApiIdentifiedPathwayUrl(pathwayNo, gender, age), Times.Once);
            _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);

            Assert.That(result, Is.EqualTo(resultString));
        }
    }
}
