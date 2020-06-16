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
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Headache", TestScenarioSex.Female, TestScenarioAgeGroups.Adult);

            questionPage.Verify();
            questionPage.VerifyRationale();
        }

        [Test]
        public void QuestionPage_DisplaysPreviousQuestionLink_OnSecondQuestion()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Headache", TestScenarioSex.Female, TestScenarioAgeGroups.Adult);
            questionPage.VerifyPreviousButton(true);

            var secondQuestion = questionPage.AnswerYes();
            secondQuestion.VerifyPreviousButton();
        }

        [Test]
        public void QuestionPage_DisplaysQuestionAdditionalInfo()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Sexual Concerns", TestScenarioSex.Male,
                    TestScenarioAgeGroups.Adult)
                .Answer(3);
            questionPage.VerifyAdditionalInfo();
        }


    }
}
