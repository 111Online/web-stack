using AutoMapper;
using Moq;
using NHS111.Models.Models.Web.Enums;
using NUnit.Framework;
using NHS111.Web.Presentation.Builders;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture]
    public class JourneyViewModelStateBuilderTests
    {
        Mock<IMappingEngine> _mappingEngine;

        [SetUp]
        public void Setup()
        {
            _mappingEngine = new Mock<IMappingEngine>();
        }

        [Test]
        public void BuildState()
        {
            _mappingEngine.Setup(x => x.Map<AgeCategory>(It.IsAny<int>())).Returns(AgeCategory.Adult);

            var result = JourneyViewModelStateBuilder.BuildState("Male", 30, _mappingEngine.Object);
            Assert.AreEqual("30", result["PATIENT_AGE"]);
            Assert.AreEqual("\"M\"", result["PATIENT_GENDER"]);
            Assert.AreEqual("1",result["PATIENT_PARTY"]);
            Assert.AreEqual("Adult", result["PATIENT_AGEGROUP"]);
        }
    }
}
