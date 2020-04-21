//using System.Collections.Generic;
//using System.Net;
//using System.Threading.Tasks;
//using Moq;
//using NHS111.Domain.Api.Controllers;
//using NHS111.Domain.Repository;
//using NHS111.Models.Models.Domain;
//using NUnit.Framework;

////TODO reviewed

namespace NHS111.Domain.Test.API_Project.Controller
{
    using Api.Controllers;
    using Domain.Repository;
    using Models.Models.Domain;
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class CareAdviceControllerTests
    {
        private Mock<ICareAdviceRepository> _careAdviceRepository;

        [SetUp]
        public void SetUp()
        {
            _careAdviceRepository = new Mock<ICareAdviceRepository>();
        }

        [Test]
        public async void GetCareAdvice_WithLowerCaseArgs_PassesStronglyTypedArgsToRepo()
        {
            var sut = new CareAdviceController(_careAdviceRepository.Object);
            await sut.GetCareAdvice("dx666", "adult", "female", "keyword1|keyword2");

            _careAdviceRepository.Verify(r => r.GetCareAdvice(It.Is<AgeCategory>(a => a.Value == "Adult"), It.Is<Gender>(g => g.Value == "Female"), It.IsAny<IEnumerable<string>>(), It.Is<DispositionCode>(d => d.Value == "Dx666")));
        }

        //        [Test]
        //        public async void should_return_a_care_advice_suggestion()
        //        {
        //            //Arrange

        //            var markers = new List<string> { "one", "two", "three" };
        //            var pathwayNo = "P1234";
        //            IEnumerable<CareAdvice> careAdviceList = new List<CareAdvice>();
        //            var markersString = "one,two,three";
        //            _careAdviceRepository.Setup(x => x.GetCareAdvice(pathwayNo, markers)).Returns(Task.FromResult(careAdviceList));

        //            var ctr = new CareAdviceController(_careAdviceRepository.Object);

        //            //Act
        //            var result = await ctr.GetCareAdvice(pathwayNo, markersString);

        //            //Assert
        //            _careAdviceRepository.Verify(x => x.GetCareAdvice(pathwayNo, markers), Times.Once);
        //            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //        }
    }
}