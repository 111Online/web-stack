namespace NHS111.DOS.Functional.Tests {
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Models.Business;
    using Models.Models.Domain;
    using Models.Models.Web.ITK;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using TestBenchApi;
    using Web.Functional.Utils;

    [Category("Local")]
    public class Call999CallbackTests
        : BaseTests {

        //fix test bench service to support running all tests at once
        //itk dispatch request factory

        [SetUp]
        public void Setup() {
            _testBench = new TestBench();
        }

        [Test]
        public async Task SubmitReferralForNonValidation999_Always_SendsCorrectSurveyData()
        {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.EmptyServiceList)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)

                .BeginAsync();

            var outcomePage = NavigateTo999Cat3(dosScenario.Postcode);
            var surveyUrl = outcomePage.SurveyLink.GetAttribute("href");
            Assert.True(surveyUrl.Contains("validation_callback_offered=false"));
            var dosResult = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task SubmitReferralForValidation999_Always_SendsCorrectSurveyData()
        {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var outcomePage = NavigateTo999Cat3(dosScenario.Postcode);
            var surveyUrl = outcomePage.SurveyLink.GetAttribute("href");
            Assert.True(surveyUrl.Contains("validation_callback_offered=true"));
            var dosResult = await _testBench.Verify(dosScenario);
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
            callbackPage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = callbackPage.AcceptCallback();

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

            postcodePage.VerifyIsCallbackAcceptancePage();
            Assert.True(Driver.ElementExists(By.Id("FindService_CurrentPostcode")),
                "Expected postcode field when no gate.");
            var personalDetailsPage = postcodePage.EnterPostCodeAndSubmit(dosScenario.Postcode);

            Assert.Fail();

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
            postcodePage.VerifyIsCallbackAcceptancePage();
            Assert.True(Driver.ElementExists(By.Id("FindService_CurrentPostcode")),
                "Expected postcode field when no gate.");
            var outcome = postcodePage.EnterPostCodeAndSubmit(dosScenario.Postcode);
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
            callbackPage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = callbackPage.AcceptCallback();

            Assert.Fail();
            //var referralConfirmation = personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsSuccessfulReferral();
            //referralConfirmation.VerifyNoCareAdvice();
            //referralConfirmation.VerifyNoWorseningAdvice();
            //SaveScreenAsPNG("999-reval-successful-referral");

            //var dosVerifyResult = await _testBench.Verify(dosScenario);
            //var esbVerifyResult = await _testBench.Verify(esbScenario);
        }

        [Test]
        public async Task SubmittingReferralForCat3_WithoutPostcode_SendsDx333ToESB() {
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
                .Matching(new ITKDispatchRequest
                {
                    CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx333.Value },
                    PatientDetails = new PatientDetails
                    { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } }
                })
                .Returns(EsbStatusCode.Success200)
                .OtherwiseReturns(EsbStatusCode.Error500)
                .BeginAsync();

            var callbackPage = NavigateTo999Cat3(null);
            callbackPage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = EnterPostCodeAndSubmit(dosScenario.Postcode);
            Assert.Fail();
            //var referralConfirmation = personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsSuccessfulReferral();

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
            callbackPage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = callbackPage.AcceptCallback();

            Assert.Fail();
            //var referralConfirmation = personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsUnsuccessfulReferral();
            //referralConfirmation.VerifyNoCareAdvice();
            //referralConfirmation.VerifyNoWorseningAdvice();
            //SaveScreenAsPNG("999-reval-unsuccessful-referral");

            var result = await _testBench.Verify(dosScenario);
            var esbVerifyResult = await _testBench.Verify(esbScenario);
        }

        [Test]
        public async Task SubmittingReferralForCat3_WithDuplicateReferral_ShowDuplicatePage() {
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
                    CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx333.Value },
                    PatientDetails = new PatientDetails
                    { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } }
                })
                .Returns(EsbStatusCode.Duplicate409)
                .OtherwiseReturns(EsbStatusCode.Success200)
                .BeginAsync();

            var callbackPage = NavigateTo999Cat3(dosScenario.Postcode);
            callbackPage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = callbackPage.AcceptCallback();

            
            Assert.Fail();
            //var referralConfirmation = personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsDuplicateReferral();
            //referralConfirmation.VerifyNoCareAdvice();
            //referralConfirmation.VerifyNoWorseningAdvice();
            //SaveScreenAsPNG("999-reval-duplicate-referral");

            //var dosRsult = await _testBench.Verify(dosScenario);
            //var esbResult = await _testBench.Verify(esbScenario);
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
            callbackPage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = callbackPage.AcceptCallback();

            Assert.Fail();
            //var referralConfirmation = personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsServiceUnavailableReferral();
            //referralConfirmation.VerifyNoCareAdvice();
            //referralConfirmation.VerifyNoWorseningAdvice();
            //SaveScreenAsPNG("999-reval-unavailable-referral");

            //var result = await _testBench.Verify(dosScenario);
            //var esbVerifyResult = await _testBench.Verify(esbScenario);
        }

        [Test]
        public async Task EDOutcome_WhenDosIsUnavailable_ShowsCorrectScreen() {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx333))
                .Returns(ServicesTransformedTo.ServerError)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var outcome = NavigateTo999Cat3(dosScenario.Postcode);
            outcome.VerifyOutcome(OutcomePage.Cat3999Text);

            var result = await _testBench.Verify(dosScenario);
        }

        private TestBench _testBench;

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

            return questionPage
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 5)
                .Answer<OutcomePage>("No");
        }

        private OutcomePage NavigateTo999Cat3(Postcode postcode) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult, postcode.Value);

            return questionPage
                .Answer(1)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .Answer<OutcomePage>("Yes");
        }

        private OutcomePage NavigateTo999Cat2() {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver,
                "Breathing Problems, Breathlessness or Wheeze",
                TestScenerioSex.Male, TestScenerioAgeGroups.Child);

            return questionPage
                .Answer(3)
                .Answer(1)
                .Answer(4)
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .Answer<OutcomePage>("Yes");
        }

        private OutcomePage EnterPostCodeAndSubmit(string postcode)
        {
            var postcodeField = Driver.FindElement(By.Id("FindService_CurrentPostcode"));
            postcodeField.Clear();
            postcodeField.SendKeys(postcode);
            Driver.FindElement(By.Id("DosLookup")).Click();
            return new OutcomePage(Driver);
        }

    }

}
