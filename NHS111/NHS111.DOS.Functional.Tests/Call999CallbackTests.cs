namespace NHS111.DOS.Functional.Tests {
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Models.Business;
    using Models.Models.Domain;
    using Models.Models.Web.ITK;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using TestBenchApi;
    using Web.Functional.Utils;

    public class Call999CallbackTests
        : BaseTests {

        //fix test bench service to support running all tests at once
        //refactor test code to reduce duplication
        //itk dispatch request factory

        [SetUp]
        public void Setup() {
            _testBench = new TestBench();
        }

        [Test]
        public async Task Call999Cat3_WithoutCallbackReturned_DisplaysOriginalDispo() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.EmptyServiceList)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var outcome = NavigateTo999Cat3(dosScenario.Postcode);

            outcome.VerifyOutcome(OutcomePage.Cat3999Text);

            var result = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task Call999Cat4_WithCallbackReturned_DisplaysPersonalDetailsPage() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var callbackPage = NavigateTo999Cat4(dosScenario.Postcode);

            AssertIsCallbackAcceptancePage(callbackPage);
            var personalDetailsPage = AcceptCallback();
            AssertIsPersonalDetailsPage(personalDetailsPage);

            var result = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task Call999Cat4_WithoutCallbackReturned_DisplaysOriginalOutcome() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.EmptyServiceList)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var outcome = NavigateTo999Cat4(dosScenario.Postcode);

            outcome.VerifyOutcome(OutcomePage.Cat4999Text);

            var result = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task Call999Cat2_Never_OffersCallback() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingNoRequestsTo(DosEndpoint.CheckCapacitySummary)
                .BeginAsync();

            var outcomePage = NavigateTo999Cat2();
            outcomePage.VerifyOutcome(OutcomePage.Cat2999Text);

            var result = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task Call999Cat3_TypingPostcodeWithCallbacks_RedirectsToPersonalDetails() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var postcodePage = NavigateTo999Cat3(null);

            AssertIsCallbackAcceptancePage(postcodePage);
            Assert.True(Driver.ElementExists(By.Id("FindService_CurrentPostcode")),
                "Expected postcode field when no gate.");
            var personalDetailsPage = SubmitPostcode(dosScenario.Postcode, postcodePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);

            var result = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task Call999Cat3_TypingPostcodeWithoutCallbacks_RedirectsToOriginalOutcome() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.EmptyServiceList)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var postcodePage = NavigateTo999Cat3(null);
            AssertIsCallbackAcceptancePage(postcodePage);
            Assert.True(Driver.ElementExists(By.Id("FindService_CurrentPostcode")),
                "Expected postcode field when no gate.");
            var outcome = SubmitPostcode(dosScenario.Postcode, postcodePage);
            outcome.VerifyOutcome(OutcomePage.Cat3999Text);

            var result = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task SubmittingReferralForCat3_WhenSuccessful_ShowsConfirmationScreen() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest {
                    CaseDetails = new CaseDetails {DispositionCode = DispositionCode.Dx333.Value},
                    PatientDetails = new PatientDetails
                        {CurrentAddress = new Address {PostalCode = dosScenario.Postcode}}
                })
                .Returns(EsbStatusCode.Success200)
                .OtherwiseReturns(EsbStatusCode.Error500)
                .BeginAsync();

            var callbackPage = NavigateTo999Cat3(dosScenario.Postcode);
            AssertIsCallbackAcceptancePage(callbackPage);
            var personalDetailsPage = AcceptCallback();
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsSuccessfulReferral(itkConfirmation);

            var dosVerifyResult = await _testBench.Verify(dosScenario);
            var esbVerifyResult = await _testBench.Verify(esbScenario);
        }

        [Test]
        public async Task SubmittingReferralForCat3_WhenUnsuccessful_ShowsFailureScreen() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest {
                    CaseDetails = new CaseDetails {DispositionCode = DispositionCode.Dx333.Value},
                    PatientDetails = new PatientDetails
                        {CurrentAddress = new Address {PostalCode = dosScenario.Postcode}}
                })
                .Returns(EsbStatusCode.Error500)
                .OtherwiseReturns(EsbStatusCode.Success200)
                .BeginAsync();

            var callbackPage = NavigateTo999Cat3(dosScenario.Postcode);
            AssertIsCallbackAcceptancePage(callbackPage);
            var personalDetailsPage = AcceptCallback();
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsUnsuccessfulReferral(itkConfirmation);

            var result = await _testBench.Verify(dosScenario);
            var esbVerifyResult = await _testBench.Verify(esbScenario);
        }

        [Test]
        public async Task SubmittingReferralForCat3_WhenServiceUnavailable_ShowsUnavailableScreen() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.EmptyServiceList)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingNoRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest {
                    CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx333.Value },
                    PatientDetails = new PatientDetails
                        { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } }
                })
                .BeginAsync();

            var callbackPage = NavigateTo999Cat3(dosScenario.Postcode);
            AssertIsCallbackAcceptancePage(callbackPage);
            var personalDetailsPage = AcceptCallback();
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsServiceUnavailableReferral(itkConfirmation);

            var result = await _testBench.Verify(dosScenario);
            var esbVerifyResult = await _testBench.Verify(esbScenario);
        }

        private TestBench _testBench;

        private OutcomePage SubmitPersonalDetails(OutcomePage personalDetailsPage) {
            Driver.FindElement(By.Id("PatientInformantDetails_Informant_Self")).Click();
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            var forename =
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("PatientInformantDetails_SelfName_Forename")));
            forename.SendKeys("Adam Test");
            Driver.FindElement(By.Id("PatientInformantDetails_SelfName_Surname")).SendKeys("Adam Test");
            Driver.FindElement(By.Id("UserInfo_TelephoneNumber")).SendKeys("07780555555");
            Driver.FindElement(By.Id("UserInfo_Day")).SendKeys("01");
            Driver.FindElement(By.Id("UserInfo_Month")).SendKeys("01");
            Driver.FindElement(By.Id("UserInfo_Year")).SendKeys("1980");
            Driver.FindElement(By.CssSelector(".button--next.button--secondary.find-address")).Click();
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            var addressPicker =
                wait.Until(ExpectedConditions.ElementIsVisible(
                    By.Id("AddressInformation_PatientCurrentAddress_SelectedAddressFromPicker")));
            var selectElement = new SelectElement(addressPicker);
            var address = selectElement.Options[1].Text;
            selectElement.SelectByText(address);
            Driver.FindElement(By.Id("home-address-dont-know")).Click();
            Driver.FindElement(By.Id("submitDetails")).Click();

            return new OutcomePage(Driver);
        }

        private OutcomePage NavigateTo999Cat4(Postcode postcode) {
            var args = postcode != null
                ? EncryptArgs(new Dictionary<string, string>
                    {{"postcode", postcode.Value}, {"sessionId", Guid.NewGuid().ToString()}})
                : null;
            return NavigateTo999Cat4WithArgs(args);
        }

        private OutcomePage NavigateTo999Cat4WithArgs(string args) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Finger or Thumb Injury, Penetrating",
                TestScenerioSex.Male, TestScenerioAgeGroups.Adult, args);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 5)
                .AnswerForDispostion<OutcomePage>("No");
            return outcomePage;
        }

        private OutcomePage NavigateTo999Cat3(Postcode postcode) {
            var args = postcode != null
                ? EncryptArgs(new Dictionary<string, string>
                    {{"postcode", postcode.Value}, {"sessionId", Guid.NewGuid().ToString()}})
                : null;
            return NavigateTo999Cat3WithArgs(args);
        }

        private OutcomePage NavigateTo999Cat3WithArgs(string args) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult, args);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");
            return outcomePage;
        }

        private OutcomePage NavigateTo999Cat2() {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver,
                "Breathing Problems, Breathlessness or Wheeze",
                TestScenerioSex.Male, TestScenerioAgeGroups.Child);

            var outcomePage = questionPage
                .Answer(3)
                .Answer(1)
                .Answer(4)
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("Yes");
            return outcomePage;
        }

        private OutcomePage SubmitPostcode(string postcode, OutcomePage postcodePage) {
            Assert.True(Driver.ElementExists(By.Id("FindService_CurrentPostcode")), "Postcode field not available");
            Driver.FindElement(By.Id("FindService_CurrentPostcode")).Clear();
            Driver.FindElement(By.Id("FindService_CurrentPostcode")).SendKeys(postcode);
            Driver.FindElement(By.Id("DosLookup")).Click();
            return new OutcomePage(Driver);
        }

        private OutcomePage AcceptCallback() {
            Driver.FindElement(By.Id("next")).Click();
            return new OutcomePage(Driver);
        }

        private void AssertIsCallbackAcceptancePage(OutcomePage outcomePage) {
            outcomePage.VerifyOutcome("A nurse needs to phone you");
        }

        private void AssertIsPersonalDetailsPage(OutcomePage personalDetailsPage) {
            personalDetailsPage.VerifyOutcome("Enter details");
        }

        private void AssertIsSuccessfulReferral(OutcomePage itkConfirmation) {
            Assert.IsTrue(Driver.ElementExists(By.CssSelector("h1")),
                "Possible unexpected triage outcome. Expected header to exist but it doesn't.");
            var header = Driver.FindElement(By.CssSelector("h1"));
            Assert.IsTrue(header.Text.StartsWith("You should get a call within"),
                string.Format(
                    "Possible unexpected triage outcome. Expected header text of 'You should get a call within' but was '{0}'.",
                    header.Text));
        }

        private void AssertIsUnsuccessfulReferral(OutcomePage itkConfirmation) {
            itkConfirmation.VerifyOutcome("Sorry, there is a problem with the service");
        }

        private void AssertIsServiceUnavailableReferral(OutcomePage itkConfirmation) {
            itkConfirmation.VerifyOutcome("Sorry, there is a problem with the service");
        }

        private void AssertIsDuplicateReferral(OutcomePage itkConfirmation) {
            itkConfirmation.VerifyOutcome("Your call has already been booked");
        }

    }

}