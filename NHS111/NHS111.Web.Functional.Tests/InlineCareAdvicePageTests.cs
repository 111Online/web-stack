using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class InlineCareAdvicePageTests : BaseTests
    {
        [Test]
        public void InlineCareAdvice_BeforeOutcome_DisplaysPage()
        {
            var inlineAdvice = GoToInlineCareAdvicePageBeforeOutcome();

            inlineAdvice.Verify();
        }

        [Test]
        public void InlineCareAdvice_BetweenQuestions_DisplaysPage()
        {
            var inlineAdvice = GoToInlineCareAdviceInBetweenQuestions();
            inlineAdvice.Verify();

            var question = inlineAdvice.MoveNext<QuestionPage>();
            question.Verify();
        }

        [Test]
        public void InlineCareAdvice_MoveNext()
        {
            var inlineAdvice = GoToInlineCareAdvicePageBeforeOutcome();
            var outcome = inlineAdvice.MoveNext<OutcomePage>();
            outcome.VerifyOutcome("See your dentist urgently");
        }

        [Test]
        public void InlineCareAdvice_MovePrevious()
        {
            var inlineAdvice = GoToInlineCareAdvicePageBeforeOutcome();
            var question = inlineAdvice.MovePrevious<QuestionPage>();
            question.Verify();
        }

        private InlineCareAdvicePage GoToInlineCareAdvicePageBeforeOutcome()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Dental Injury", TestScenarioSex.Female, TestScenarioAgeGroups.Adult);
            return questionPage.Answer(3)
                .Answer(5)
                .Answer(1)
                .Answer(1)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .Answer(1)
                .Answer<InlineCareAdvicePage>(3);
        }

        private InlineCareAdvicePage GoToInlineCareAdviceInBetweenQuestions()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Burn, Chemical", TestScenarioSex.Male, TestScenarioAgeGroups.Adult);
            return questionPage.Answer(5)
                .Answer(3)
                .Answer<InlineCareAdvicePage>(3);
        }
    }
}
