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
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Injury", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);
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
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Burn, Chemical", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            return questionPage.Answer(5)
                .Answer(3)
                .Answer<InlineCareAdvicePage>(3);
        }
    }
}
