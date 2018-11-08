
namespace NHS111.DOS.Functional.Tests {
    using NUnit.Framework;
    using OpenQA.Selenium;
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

        [Test] //no postcode present
        public void EDOutcome_WhenNoPostcodePresent_ShowsPostcodePageAndThenPersonalDetailsPage() {
            var postcodePage = NavigateToRemappedEDOutcome();
            AssertIsPostcodePage(postcodePage);
            var callbackAcceptancePage = EnterPostcode("CA2 7HY", postcodePage);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var edOutcome = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(edOutcome);
        }

        [Test] //requires OX1 1DJ
        public void Dx94_WithNoCallbackServices_ShowOriginalOutcome() {
            var edOutcome = NavigateToDx94Outcome();
            AssertIsOriginalOutcome(edOutcome, "Dx94");
        }

        private OutcomePage RejectCallback(OutcomePage callbackAcceptancePage) {
            Driver.FindElement(By.Id("No")).Click();
            Driver.FindElement(By.Id("Next")).Click();
            return new OutcomePage(Driver);
        }

        private OutcomePage AcceptCallback(OutcomePage callbackAcceptancePage) {
            Driver.FindElement(By.Id("Yes")).Click();
            Driver.FindElement(By.Id("Next")).Click();
            return new OutcomePage(Driver);
        }

        private OutcomePage EnterPostcode(string postcode, OutcomePage postcodePage) {
            Driver.FindElement(By.Id("CurrentPostcode")).Clear();
            Driver.FindElement(By.Id("CurrentPostcode")).SendKeys(postcode);
            Driver.FindElement(By.Id("postcode")).Click();
            return new OutcomePage(Driver);
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

        private OutcomePage NavigateToDx94Outcome() {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Sexual or Menstrual Concerns", TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult);

            return questionPage.AnswerForDispostion<OutcomePage>(1);
        }

        private void AssertIsOriginalOutcome(OutcomePage edOutcome, string expectedDisposition = "Dx02") {
            edOutcome.VerifyDispositionCode(expectedDisposition);
        }

        private void AssertIsPersonalDetailsPage(OutcomePage edOutcome) {
            edOutcome.VerifyOutcome("Enter details");
        }

        private void AssertIsPostcodePage(OutcomePage edOutcome) {
            edOutcome.VerifyOutcome("Where do you want help?");
        }

        private void AssertIsCallbackAcceptancePage(OutcomePage edOutcome) {
            edOutcome.VerifyOutcome("Get a phone call from a nurse");
        }
    }
}