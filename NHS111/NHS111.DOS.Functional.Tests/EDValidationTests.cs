
namespace NHS111.DOS.Functional.Tests {
    using NUnit.Framework;
    using SmokeTest.Utils;
    using SmokeTests;

    [TestFixture(Description = "Tests the callback/validation flow for Emergency Department outcomes.")]
    public class EDValidationTests
        : BaseTests {

        [Test]
        [Ignore("Currently all ED dxcodes are remapped")]
        public void EDOutcome_NotMappedToDx334_ShowsOriginalOutcome() {
            var edOutcome = NavigateToNonRemappedEDOutcome();
            AssertIsOriginalOutcome(edOutcome);
        }

        [Test] //requires NO callback services returned for this scenario
        public void EDOutcome_MappedToDx334ButNoCallbackServicesReturned_ShowsOriginalOutcome() {
            var edOutcome = NavigateToRemappedEDOutcome();
            AssertIsOriginalOutcome(edOutcome);
        }

        [Test]
        public void EDOutcome_MappedToDx334AndReturnsCallbackServices_ShowsCallbackAcceptancePage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome();
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
        }

        [Test]
        public void EDOutcome_WhenUserRejectsCallbackOffer_ShowsOriginalOutcome() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome();
            var edOutcome = RejectCallback(callbackAcceptancePage);
            AssertIsOriginalOutcome(edOutcome);
        }

        [Test]
        public void EDOutcome_WhenUserAcceptsCallbackOffer_ShowsPersonalDetailsPage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome();
            var edOutcome = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(edOutcome);
        }

        [Test]
        public void EDOutcome_WhenNoPostcodePresent_ShowsPostcodePageAndThenPersonalDetailsPage() {
            var postcodePage = NavigateToRemappedEDOutcome();
            AssertIsPostcodePage(postcodePage);
            var callbackAcceptancePage = EnterPostcode("LS17 7NZ", postcodePage);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var edOutcome = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(edOutcome);
        }

        private OutcomePage RejectCallback(OutcomePage callbackAcceptancePage) {
            throw new System.NotImplementedException();
        }

        private OutcomePage AcceptCallback(OutcomePage callbackAcceptancePage) {
            throw new System.NotImplementedException();
        }

        private OutcomePage EnterPostcode(string postcode, OutcomePage postcodePage) {
            throw new System.NotImplementedException();
        }

        private OutcomePage NavigateToNonRemappedEDOutcome() {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult);

            return questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerForDispostion<OutcomePage>("Yes");
        }

        private OutcomePage NavigateToRemappedEDOutcome() {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult);

            return questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerForDispostion<OutcomePage>("Yes");
        }

        private void AssertIsOriginalOutcome(OutcomePage edOutcome) {
            edOutcome.VerifyDispositionCode("Dx02");
        }

        private void AssertIsPersonalDetailsPage(OutcomePage edOutcome) {
            Assert.True(false);
        }

        private void AssertIsPostcodePage(OutcomePage edOutcome) {
            Assert.True(false);
        }

        private void AssertIsCallbackAcceptancePage(OutcomePage edOutcome) {
            Assert.True(false);
        }
    }
}