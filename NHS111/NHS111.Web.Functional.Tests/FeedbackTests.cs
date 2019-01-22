using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class FeedbackTests : BaseTests
    {
        [Test]
        public void Feedback_Displays()
        {
            var moduleZero = TestScenarioPart.ModuleZero(Driver);
            moduleZero.VerifyFeedbackDisplayed();
        }

        [Test]
        public void Feedback_Submit()
        {
            var moduleZero = TestScenarioPart.ModuleZero(Driver);
            moduleZero.VerifyFeedbackSubmits();
        }
    }
}
