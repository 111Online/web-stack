using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
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
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Skin Problems", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyAdditionalInfo();
        }


    }
}
