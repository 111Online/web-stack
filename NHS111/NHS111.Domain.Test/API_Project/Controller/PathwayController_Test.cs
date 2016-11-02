using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NHS111.Domain.Api.Controllers;
using NHS111.Domain.Repository;
using NHS111.Models.Models.Domain;
using NUnit.Framework;

namespace NHS111.Domain.Test.API_Project.Controller
{
    [TestFixture]
    public class PathwayController_Test
    {
        Mock<IPathwayRepository> _pathwayRepository;
        private PathwayController _sut;

        [SetUp]
        public void SetUp()
        {
            _pathwayRepository = new Mock<IPathwayRepository>();
            _sut = new PathwayController(_pathwayRepository.Object);
        }

        [Test]
        public async void should_return_unique_pathway_when_parameter_is_true()
        {
            //Arrange
            IEnumerable<GroupedPathways> pathwayList = new List<GroupedPathways>();
            _pathwayRepository.Setup(x => x.GetGroupedPathways(true)).Returns(Task.FromResult(pathwayList));

            //Act
            var result = await _sut.GetPathways(grouped:true,startingOnly:true);

            //Assert
            _pathwayRepository.Verify(x => x.GetGroupedPathways(true), Times.Once);
            _pathwayRepository.Verify(x => x.GetAllPathways(true), Times.Never);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        [Test]
        public async void should_return_collection_of_pathway_when_parameter_is_false()
        {
            //Arrange
            IEnumerable<Pathway> pathwayList = new List<Pathway>();
            _pathwayRepository.Setup(x => x.GetAllPathways(true)).Returns(Task.FromResult(pathwayList));

            var sut = new PathwayController(_pathwayRepository.Object);

            //Act
            var result = await sut.GetPathways(grouped: false, startingOnly: true);

            //Assert
            _pathwayRepository.Verify(x => x.GetGroupedPathways(true), Times.Never);
            _pathwayRepository.Verify(x => x.GetAllPathways(true), Times.Once);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        [Test]
        public async void should_return_a_single_pathway()
        {
            //Arrange
             
            var id = "PW1234";
           
            _pathwayRepository.Setup(x => x.GetPathway(id)).Returns(Task.FromResult(new Pathway()));

            //Act
            var result = await _sut.GetPathway(id);

            //Assert
            _pathwayRepository.Verify(x => x.GetPathway(id), Times.Once);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }

        [Test]
        public async void should_return_an_identified_pathway()
        {
            //Arrange

            var pathwayNo = new[] { "PW101" };
            var gender = "male";
            var age = 35;

            _pathwayRepository.Setup(x => x.GetIdentifiedPathway(pathwayNo, gender, age)).Returns(Task.FromResult(new Pathway()));

            //Act
            var result = await _sut.GetIdentifiedPathway(string.Join(",", pathwayNo), gender, age);

            //Assert
            _pathwayRepository.Verify(x => x.GetIdentifiedPathway(pathwayNo, gender, age), Times.Once);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        }
    }
}