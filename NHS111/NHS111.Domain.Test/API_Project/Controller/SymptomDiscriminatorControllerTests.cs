
namespace NHS111.Domain.Test.API_Project.Controller {
    using System.Threading.Tasks;
    using Api.Controllers;
    using Domain.Repository;
    using Models.Models.Domain;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SymptomDiscriminatorControllerTests {

        private Mock<ISymptomDiscriminatorRepository> _mockRepo;

        [SetUp]
        public void SetUp() {
            _mockRepo = new Mock<ISymptomDiscriminatorRepository>();
        }

        [Test]
        public async void Get_WithExistingSDCode_ReturnsCorrectSD() {
            _mockRepo.Setup(r => r.Get(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult((new SymptomDiscriminator {Id = i})));

            var sut = new SymptomDiscriminatorController(_mockRepo.Object);
            var sd = await sut.Get(123);

            Assert.AreEqual(123, sd.Content.Id);
        }

        [Test]
        public async void Get_WithNonexistingSDCode_ReturnsNull() {
            _mockRepo.Setup(r => r.Get(It.IsAny<int>()))
                .Returns((int i) => Task.FromResult<SymptomDiscriminator>(null));

            var sut = new SymptomDiscriminatorController(_mockRepo.Object);
            var sd = await sut.Get(123);

            Assert.IsNull(sd.Content);
        }

    }
}