namespace NHS111.DOS.Functional.Tests {
    using NUnit.Framework;
    using SmokeTest.Utils;

    [TestFixture()]
    public class Call999CallbackTests
    : BaseTests {

        [TestCase(OutcomePage.Cat3999Text)] //callback returned
        //[TestCase(OutcomePage.Call999CallbackText)] //no callback returned
        public void Call999Cat3_WithDosResult_DisplaysExpectedDispositionPage(string expectedOutcomeText) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerForDeadEnd<OutcomePage>("Yes");

            outcomePage.VerifyOutcome(expectedOutcomeText);
        }

        [TestCase(OutcomePage.Cat4999Text)] //callback returned
        //[TestCase(OutcomePage.Call999CallbackText)] //no callback returned
        public void Call999Cat4_WithDosResult_DisplaysExpectedDispositionPage(string expectedOutcomeText)
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Finger or Thumb Injury, Penetrating", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 5)
                .AnswerForDeadEnd<OutcomePage>("No");

            outcomePage.VerifyOutcome(expectedOutcomeText);
        }
    }
}
