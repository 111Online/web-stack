using NUnit.Framework;
using NHS111.Web.Presentation.Builders;

namespace NHS111.Web.Presentation.Test.Builders
{
    [TestFixture]
    public class JourneyViewModelStateBuilderTests
    {
        [Test]
        public void BuildStateMaleAdult()
        {
            var result = JourneyViewModelStateBuilder.BuildState("", "Male", 30);
            Assert.AreEqual("30", result["PATIENT_AGE"]);
            Assert.AreEqual("\"M\"", result["PATIENT_GENDER"]);
            Assert.AreEqual("1", result["PATIENT_PARTY"]);
            Assert.AreEqual("Adult", result["PATIENT_AGEGROUP"]);
        }
    }
}
