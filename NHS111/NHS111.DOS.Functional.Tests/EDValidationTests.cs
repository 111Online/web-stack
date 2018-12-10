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

    /// Tests the callback/validation flow for Emergency Department outcomes.
    public class EDValidationTests
        : BaseTests {

        [SetUp]
        public void Setup() {
            _testBench = new TestBench();
        }

        [Test]
        public async Task SubmitReferralForDx02_AfterNoResultsFor334_SendsDx02ToESB() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.EmptyServiceList)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx02))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx02))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest {CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx02.Value }, PatientDetails = new PatientDetails{CurrentAddress = new Address{PostalCode = dosScenario.Postcode}}})
                .Returns(EsbStatusCode.Success200)
                .OtherwiseReturns(EsbStatusCode.Error500)
                .BeginAsync();

            var edOutcome = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            AssertIsOriginalOutcome(edOutcome);
            var personalDetailsPage = ClickBookCallButton(edOutcome);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsSuccessfulReferral(itkConfirmation);
            SaveScreenAsPNG("ed-reval-successful-referral");

            var result = await _testBench.Verify(dosScenario);
            Assert.IsInstanceOf<SuccessfulVerificationResult>(result);
        }

        [Test]
        public async Task SubmitReferralForDx334_Always_SendsDx334ToESB()
        {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest { CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx334.Value }, PatientDetails = new PatientDetails { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } } })
                .Returns(EsbStatusCode.Success200)
                .OtherwiseReturns(EsbStatusCode.Error500)
                .BeginAsync();

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var personalDetailsPage = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsSuccessfulReferral(itkConfirmation);

            var result = await _testBench.Verify(dosScenario);
            Assert.IsInstanceOf<SuccessfulVerificationResult>(result);
        }

        [Test] //no postcode present
        public async Task EDOutcome_ThenEnteringPostcodeReturningCallback_ShowsCallbackThenPersonalDetailsPage() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var postcodePage = NavigateToRemappedEDOutcomeWithArgs(null);
            AssertIsPostcodePage(postcodePage);
            var callbackAcceptancePage = EnterPostcode(dosScenario.Postcode, postcodePage);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var edOutcome = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(edOutcome);

            var result = await _testBench.Verify(dosScenario);
            Assert.IsInstanceOf<SuccessfulVerificationResult>(result);
        }

        [Test]
        public async Task Dx94_WithNoCallbackServices_ShowOriginalOutcome() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingNoRequestsTo(DosEndpoint.CheckCapacitySummary)
                .BeginAsync();
            
            var edOutcome = NavigateToDx94Outcome();
            AssertIsOriginalOutcome(edOutcome, "Dx94", "Your answers suggest you should go to A&E within 1 hour");

            var result = await _testBench.Verify(dosScenario);
            Assert.IsInstanceOf<SuccessfulVerificationResult>(result);
        }

        [Test]
        public async Task SubmittingReferralRequest_AfterRejectingDx334Callback_SubmitsReferralWithCorrectDxCode() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx02))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx02))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest { CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx02.Value }, PatientDetails = new PatientDetails { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } } })
                .Returns(EsbStatusCode.Success200)
                .OtherwiseReturns(EsbStatusCode.Error500)
                .BeginAsync();

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var edOutcome = RejectCallback(callbackAcceptancePage);
            AssertIsOriginalOutcome(edOutcome);
            var personalDetailsPage = ClickBookCallButton(edOutcome);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsSuccessfulReferral(itkConfirmation);

            var result = await _testBench.Verify(dosScenario);
            Assert.IsInstanceOf<SuccessfulVerificationResult>(result);
        }

        [Test]
        public async Task EDOutcome_WithDosErrorForFirstQuery_ReturnsResultsForSecondQuery() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.ServerError)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx02))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var edOutcome = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            AssertIsOriginalOutcome(edOutcome);
            Assert.True(Driver.ElementExists(By.Name("PersonalDetails")));

            var result = await _testBench.Verify(dosScenario);
            Assert.IsInstanceOf<SuccessfulVerificationResult>(result);
        }

        [Test]
        public async Task SubmittingReferralRequest_WithUnsuccessfulReferral_ShowUnsuccessfulPage() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest { CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx334.Value }, PatientDetails = new PatientDetails { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } } })
                .Returns(EsbStatusCode.Error500)
                .OtherwiseReturns(EsbStatusCode.Success200)
                .BeginAsync();

            //TestContext.CurrentContext.Test.Properties.Add("dosScenario", dosScenario);
            //TestContext.CurrentContext.Test.Properties.Add("esbScenario ", esbScenario);

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var personalDetailsPage = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsUnsuccessfulReferral(itkConfirmation);
            SaveScreenAsPNG("ed-reval-unsuccessful-referral");

            var result = await _testBench.Verify(dosScenario);
            Assert.IsInstanceOf<SuccessfulVerificationResult>(result);
        }

        [Test]
        public async Task SubmittingReferralRequest_WithDuplicateReferral_ShowDuplicatePage() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest { CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx334.Value }, PatientDetails = new PatientDetails { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } } })
                .Returns(EsbStatusCode.Duplicate409)
                .OtherwiseReturns(EsbStatusCode.Success200)
                .BeginAsync();

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var personalDetailsPage = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsDuplicateReferral(itkConfirmation);
            SaveScreenAsPNG("ed-reval-duplicate-referral");

            var result = await _testBench.Verify(dosScenario);
            Assert.IsInstanceOf<SuccessfulVerificationResult>(result);
        }

        [Test]
        public async Task SubmittingReferralRequest_WithUnavailableService_ShowServiceUnavailablePage() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .Then()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.EmptyServiceList)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var personalDetailsPage = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsServiceUnavailableReferral(itkConfirmation);
            SaveScreenAsPNG("ed-reval-unavailable-referral");

            var result = await _testBench.Verify(dosScenario);
            Assert.IsInstanceOf<SuccessfulVerificationResult>(result);
        }

        private TestBench _testBench;

        private void AssertReturnedServiceExists(string serviceName) {
            Assert.IsTrue(Driver.ElementExists(By.XPath(string.Format("//H3[text()='{0}']", serviceName))));
        }

        private void AssertIsSuccessfulReferral(OutcomePage itkConfirmation) {
            Assert.IsTrue(Driver.ElementExists(By.CssSelector("h1")), "Possible unexpected triage outcome. Expected header to exist but it doesn't.");
            var header = Driver.FindElement(By.CssSelector("h1"));
            Assert.IsTrue(header.Text.StartsWith("You should get a call within"), string.Format("Possible unexpected triage outcome. Expected header text of 'You should get a call within' but was '{0}'.", header.Text));
        }

        private void AssertIsUnsuccessfulReferral(OutcomePage itkConfirmation) {
            itkConfirmation.VerifyOutcome("Sorry, there is a problem with the service");
        }

        private void AssertIsDuplicateReferral(OutcomePage itkConfirmation) {
            itkConfirmation.VerifyOutcome("Your call has already been booked");
        }

        private void AssertIsServiceUnavailableReferral(OutcomePage itkConfirmation) {
            itkConfirmation.VerifyOutcome("Sorry, there is a problem with the service");
        }

        private void AssertIsOriginalOutcome(OutcomePage edOutcome, string expectedDispositionCode = "Dx02", string dispositionText = "Go to an emergency treatment centre urgently") {
            edOutcome.VerifyOutcome(dispositionText);
            edOutcome.VerifyDispositionCode(expectedDispositionCode);
        }

        private void AssertIsPersonalDetailsPage(OutcomePage personalDetailsPage) {
            personalDetailsPage.VerifyOutcome("Enter details");
        }

        private void AssertIsPostcodePage(OutcomePage edOutcome) {
            edOutcome.VerifyOutcome("Where do you want help?");
        }

        private void AssertIsCallbackAcceptancePage(OutcomePage edOutcome) {
            edOutcome.VerifyOutcome(OutcomePage.GetCallBackText);
        }

        private OutcomePage SubmitPersonalDetails(OutcomePage personalDetailsPage) {
            Driver.FindElement(By.Id("PatientInformantDetails_Informant_Self")).Click();
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            var forename = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("PatientInformantDetails_SelfName_Forename")));
            forename.SendKeys("Adam Test");
            Driver.FindElement(By.Id("PatientInformantDetails_SelfName_Surname")).SendKeys("Adam Test");
            Driver.FindElement(By.Id("UserInfo_TelephoneNumber")).SendKeys("07780555555");
            Driver.FindElement(By.Id("UserInfo_Day")).SendKeys("01");
            Driver.FindElement(By.Id("UserInfo_Month")).SendKeys("01");
            Driver.FindElement(By.Id("UserInfo_Year")).SendKeys("1980");
            Driver.FindElement(By.CssSelector(".button--next.button--secondary.find-address")).Click();
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            var addressPicker = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("AddressInformation_PatientCurrentAddress_SelectedAddressFromPicker")));
            var selectElement = new SelectElement(addressPicker);
            var address = selectElement.Options[1].Text;
            selectElement.SelectByText(address);
            Driver.FindElement(By.Id("home-address-dont-know")).Click();
            Driver.FindElement(By.Id("submitDetails")).Click();

            return new OutcomePage(Driver);
        }

        private OutcomePage ClickBookCallButton(OutcomePage edOutcome) {
            Driver.FindElement(By.Name("PersonalDetails")).Click();
            return new OutcomePage(Driver);
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

        private OutcomePage NavigateToRemappedEDOutcome(Postcode postcode) {
            var args = postcode != null
                ? EncryptArgs(new Dictionary<string, string>
                    {{"postcode", postcode.Value}, {"sessionId", Guid.NewGuid().ToString()}})
                : null;
            return NavigateToRemappedEDOutcomeWithArgs(args);
        }

        private OutcomePage NavigateToRemappedEDOutcomeWithArgs(string args) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, args);

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
    }
}
