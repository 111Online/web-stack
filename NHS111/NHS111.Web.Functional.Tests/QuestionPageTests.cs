using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class QuestionPageTests : BaseTests
    {
        [Test]
        public void QuestionPage_DisplaysPage_And_Rationale()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.Verify();
            questionPage.VerifyRationale();
        }

        [Test]
        public void QuestionPage_DisplaysPreviousQuestionLink_OnSecondQuestion()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);
            questionPage.VerifyPreviousButton(true);

            var secondQuestion = questionPage.AnswerYes();
            secondQuestion.VerifyPreviousButton();
        }

        [Test]
        public void QuestionPage_DisplaysQuestionAdditionalInfo()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Sexual Concerns", TestScenerioSex.Male,
                    TestScenerioAgeGroups.Adult)
                .Answer(3);
            questionPage.VerifyAdditionalInfo();
        }


    }
}
