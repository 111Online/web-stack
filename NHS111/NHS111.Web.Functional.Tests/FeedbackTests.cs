using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class FeedbackTests : BaseTests
    {

        private FeedbackSection GetFeedbackSection()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZero = TestScenarioPart.ModuleZero(locationPage);
            return TestScenarioPart.FeedbackSection(moduleZero);
        }

        [Test]
        public void Feedback_Displays()
        {
            var feedbackSection = GetFeedbackSection();
            feedbackSection.VerifyFeedbackDisplayed();
        }

        [Test]
        public void Feedback_DisabledWhenEmpty()
        {
            var feedbackSection = GetFeedbackSection();
            feedbackSection.TypeInTextarea("");
            feedbackSection.VerifyButtonDisabled();
        }

        [Test]
        public void Feedback_EnabledMaximumText()
        {
            var feedbackSection = GetFeedbackSection();
            var text = "";
            for (var i = 0; i < 1200; i++)
            {
                text += "a";
            }
            feedbackSection.TypeInTextarea(text);
            feedbackSection.VerifyButtonEnabled();
        }

        [Test]
        public void Feedback_DisabledTooMuchText()
        {
            var feedbackSection = GetFeedbackSection();
            var text = "";
            for (var i = 0; i < 1201; i++)
            {
                text += "a";
            }
            feedbackSection.TypeInTextarea(text);
            feedbackSection.VerifyButtonDisabled();
        }

        [Test]
        public void Feedback_Submission_TextShows()
        {
            var feedbackSection = GetFeedbackSection();
            feedbackSection.TypeInTextarea("test");
            feedbackSection.ClickSubmitButton();
            feedbackSection.VerifySuccessTextDisplayed();
        }

        [Test]
        public void Feedback_Submission_Success()
        {
            var feedbackSection = GetFeedbackSection();
            feedbackSection.TypeInTextarea("test");
            feedbackSection.ClickSubmitButton();
            feedbackSection.VerifySuccessTextCorrect();
        }
    }
}
