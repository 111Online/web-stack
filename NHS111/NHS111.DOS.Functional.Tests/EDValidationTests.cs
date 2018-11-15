
namespace NHS111.DOS.Functional.Tests {
    using NUnit.Framework;
    using OpenQA.Selenium;
    using SmokeTest.Utils;
    using SmokeTests;

    [TestFixture(Description = "Tests the callback/validation flow for Emergency Department outcomes.")]
    public class EDValidationTests
        : BaseTests {

        [Test]
        public void EDOutcome_MappedToDx334ButNoCallbackServicesReturned_ShowsOriginalOutcome() {
            var edOutcome = NavigateToRemappedEDOutcome(_postcodeWithoutCallbacks);
            AssertIsOriginalOutcome(edOutcome);
        }

        [Test]
        public void EDOutcome_MappedToDx334AndReturnsCallbackServices_ShowsCallbackAcceptancePage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_postcodeWithCallbacks);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
        }

        [Test]
        public void EDOutcome_WhenUserRejectsCallbackOffer_ShowsOriginalOutcome() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_postcodeWithCallbacks);
            var edOutcome = RejectCallback(callbackAcceptancePage);
            AssertIsOriginalOutcome(edOutcome);
        }

        [Test]
        public void EDOutcome_WhenUserAcceptsCallbackOffer_ShowsPersonalDetailsPage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_postcodeWithCallbacks);
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

        [Test]
        public void Dx94_WithNoCallbackServices_ShowOriginalOutcome() {
            var edOutcome = NavigateToDx94Outcome(_postcodeWithoutCallbacks);
            AssertIsOriginalOutcome(edOutcome, "Dx94");
        }

        [Test]
        public void SubmittingCallbackRequest_AfterRejectingDx334Callback_QueriesDosWithCorrectDxCodes()
        {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_postcodeWithCallbacks);
            var edOutcome = RejectCallback(callbackAcceptancePage);
            AssertIsOriginalOutcome(edOutcome);
            //request callback
            AssertIsPersonalDetailsPage(edOutcome);
            //fill out personal details page
            //submit
            //
        }

        private string _postcodeWithoutCallbacks =
            "432154ACCF327E1B5EC357976AD58C74959EEE81E2C8A4AB6EC9A4BE1857C53C0620D8228A87C3A7DCDAB01541DB0CE4AA11B719F7976904228B6FB10C844CC1D7846B30C9346C5A3170AE96564D2BEEDA2DF7608D8E695177D52817BB71A767";

        private string _postcodeWithCallbacks =
            "432154ACCF327E1B5EC357976AD58C74D6E91488AC242A8F2B1B7B59B04FE2028F2CF992BC570B07F73B97FC4001E4AEBABDBF2E9936E5AC9C15677471B88598E8224E931A957B174A1461F91150C30D48F1303460AB9044";

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

        private OutcomePage NavigateToRemappedEDOutcome(string args = null) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult, args);

            return questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerForDispostion<OutcomePage>("Yes");
        }

        private OutcomePage NavigateToDx94Outcome(string args) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Sexual or Menstrual Concerns", TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult, args);

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