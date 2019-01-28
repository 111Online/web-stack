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
            var feedbackSection = TestScenarioPart.FeedbackSection(Driver);
            feedbackSection.VerifyFeedbackDisplayed();
        }

        [Test]
        public void Feedback_DisabledWhenEmpty()
        {
            var feedbackSection = TestScenarioPart.FeedbackSection(Driver);
            feedbackSection.TypeInTextarea("");
            feedbackSection.VerifyButtonDisabled();
        }

        [Test]
        public void Feedback_DisabledWhenTooMuchText()
        {
            var feedbackSection = TestScenarioPart.FeedbackSection(Driver);
            var text = "";
            for (var i = 0; i < 1200; i++)
            {
                text += i;
            }
            feedbackSection.TypeInTextarea(text);
            feedbackSection.VerifyButtonDisabled();
        }

        [Test]
        public void Feedback_Submit()
        {
            var feedbackSection = TestScenarioPart.FeedbackSection(Driver);
            feedbackSection.ClickSubmitButton();
            //feedbackSection.VerifyFeedbackSubmits();
        }
    }
}
