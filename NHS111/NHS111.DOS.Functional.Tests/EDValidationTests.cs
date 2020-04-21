namespace NHS111.DOS.Functional.Tests
{
    using Models.Models.Business;
    using Models.Models.Domain;
    using Models.Models.Web.ITK;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using System.Threading.Tasks;
    using TestBenchApi;
    using Web.Functional.Utils;

    /// Tests the callback/validation flow for Emergency Department outcomes.
    [Category("Local")]
    public class EDValidationTests
        : BaseTests
    {

        [SetUp]
        public void Setup()
        {
            _testBench = new TestBench();
        }

        [Test]
        public async Task SubmitReferralForNonValidationED_Always_SendsCorrectSurveyData()
        {
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
                .BeginAsync();

            var outcomePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            var surveyUrl = outcomePage.SurveyLink.GetAttribute("href");
            Assert.True(surveyUrl.Contains("validation_callback_offered=false"));
            var dosResult = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task SubmitReferralForValidationED_Always_SendsCorrectSurveyData()
        {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var outcomePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            var surveyUrl = outcomePage.SurveyLink.GetAttribute("href");
            Assert.True(surveyUrl.Contains("validation_callback_offered=true"));
            var dosResult = await _testBench.Verify(dosScenario);
        }


        [Test]
        public async Task SubmitReferralForDx02_AfterNoResultsFor334_SendsDx02ToESB()
        {
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

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest
                {
                    CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx02.Value },
                    PatientDetails = new PatientDetails
                    { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } }
                })
                .Returns(EsbStatusCode.Success200)
                .OtherwiseReturns(EsbStatusCode.Error500)
                .BeginAsync();

            var edOutcome = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            AssertIsOriginalOutcome(edOutcome);
            var personalDetailsPage = ClickBookCallButton(edOutcome);
            personalDetailsPage.VerifyIsPersonalDetailsPage();

            Assert.Fail();

            //var referralConfirmation =
            //    personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsSuccessfulReferral();
            //SaveScreenAsPNG("ed-reval-successful-referral");

            //var dosResult = await _testBench.Verify(dosScenario);
            //var esbResult = await _testBench.Verify(esbScenario);
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

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest
                {
                    CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx334.Value },
                    PatientDetails = new PatientDetails
                    { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } }
                })
                .Returns(EsbStatusCode.Success200)
                .OtherwiseReturns(EsbStatusCode.Error500)
                .BeginAsync();

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            callbackAcceptancePage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = callbackAcceptancePage.AcceptCallback();
            personalDetailsPage.VerifyIsPersonalDetailsPage();


            Assert.Fail();
            //var referralConfirmation =
            //    personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsSuccessfulReferral();
            //referralConfirmation.VerifyCareAdviceHeader("What you can do in the meantime");
            //SaveScreenAsPNG("ed-reval-successful-referral");

            //var resultDos = await _testBench.Verify(dosScenario);
            //var resultEsb = await _testBench.Verify(esbScenario);
        }

        [Test] //no postcode present
        public async Task EDOutcome_ThenEnteringPostcodeReturningCallback_ShowsCallbackThenPersonalDetailsPage()
        {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.OnlyOneCallback)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var postcodePage = NavigateToRemappedEDOutcome(null);
            AssertIsPostcodePage(postcodePage);
            var callbackAcceptancePage = EnterPostCodeAndSubmit(dosScenario.Postcode);
            callbackAcceptancePage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = callbackAcceptancePage.AcceptCallback();
            personalDetailsPage.VerifyIsPersonalDetailsPage();

            var result = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task Dx94_WithNoCallbackServices_ShowOriginalOutcome()
        {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingNoRequestsTo(DosEndpoint.CheckCapacitySummary)
                .BeginAsync();

            var edOutcome = NavigateToDx94Outcome();
            AssertIsOriginalOutcome(edOutcome, "Dx94", "Your answers suggest you should go to A&E within 1 hour");

            var result = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task SubmittingReferralRequest_AfterRejectingDx334Callback_SubmitsReferralWithCorrectDxCode()
        {
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

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest
                {
                    CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx02.Value },
                    PatientDetails = new PatientDetails
                    { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } }
                })
                .Returns(EsbStatusCode.Success200)
                .OtherwiseReturns(EsbStatusCode.Error500)
                .BeginAsync();

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            callbackAcceptancePage.VerifyIsCallbackAcceptancePage();
            var edOutcome = callbackAcceptancePage.RejectCallback();
            AssertIsOriginalOutcome(edOutcome);

            Assert.Fail();
            //var personalDetailsPage = ClickBookCallButton(edOutcome);
            //personalDetailsPage.VerifyIsPersonalDetailsPage();
            //var referralConfirmation =
            //    personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsSuccessfulReferral();

            //var dosResult = await _testBench.Verify(dosScenario);
            //var esbResult = await _testBench.Verify(esbScenario);
        }

        [Test]
        public async Task EDOutcome_WhenDosIsUnavailable_ShowsCorrectScreen()
        {
            var dosScenario = await _testBench.SetupDosScenario()
                .ExpectingRequestTo(DosEndpoint.CheckCapacitySummary)
                .Matching(BlankDosCase.WithDxCode(DispositionCode.Dx334))
                .Returns(ServicesTransformedTo.ServerError)
                .OtherwiseReturns(DosRequestMismatchResult.ServerError)
                .BeginAsync();

            var edOutcome = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            AssertIsOriginalOutcome(edOutcome);

            var result = await _testBench.Verify(dosScenario);
        }

        [Test]
        public async Task EDOutcome_WithDosErrorForFirstQuery_ReturnsResultsForSecondQuery()
        {
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
        }

        [Test]
        public async Task SubmittingReferralRequest_WithUnsuccessfulReferral_ShowUnsuccessfulPage()
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

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest
                {
                    CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx334.Value },
                    PatientDetails = new PatientDetails
                    { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } }
                })
                .Returns(EsbStatusCode.Error500)
                .OtherwiseReturns(EsbStatusCode.Success200)
                .BeginAsync();

            //TestContext.CurrentContext.Test.Properties.Add("dosScenario", dosScenario);
            //TestContext.CurrentContext.Test.Properties.Add("esbScenario ", esbScenario);

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            callbackAcceptancePage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = callbackAcceptancePage.AcceptCallback();
            personalDetailsPage.VerifyIsPersonalDetailsPage();

            Assert.Fail();
            //var referralConfirmation =
            //    personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsUnsuccessfulReferral();
            //SaveScreenAsPNG("ed-reval-unsuccessful-referral");

            //var dosResult = await _testBench.Verify(dosScenario);
            //var esbResult = await _testBench.Verify(esbScenario);
        }

        [Test]
        public async Task SubmittingReferralRequest_WithDuplicateReferral_ShowDuplicatePage()
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

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest
                {
                    CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx334.Value },
                    PatientDetails = new PatientDetails
                    { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } }
                })
                .Returns(EsbStatusCode.Duplicate409)
                .OtherwiseReturns(EsbStatusCode.Success200)
                .BeginAsync();

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            callbackAcceptancePage.VerifyIsCallbackAcceptancePage();

            Assert.Fail();
            //var personalDetailsPage = callbackAcceptancePage.AcceptCallback();
            //personalDetailsPage.VerifyIsPersonalDetailsPage();
            //var referralConfirmation =
            //    personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsDuplicateReferral();
            //SaveScreenAsPNG("ed-reval-duplicate-referral");

            //var dosRsult = await _testBench.Verify(dosScenario);
            //var esbResult = await _testBench.Verify(esbScenario);
        }

        [Test]
        public async Task SubmittingReferralRequest_WithUnavailableService_ShowServiceUnavailablePage()
        {
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

            var esbScenario = await _testBench.SetupEsbScenario()
                .ExpectingNoRequestTo(EsbEndpoint.SendItkMessage)
                .Matching(new ITKDispatchRequest
                {
                    CaseDetails = new CaseDetails { DispositionCode = DispositionCode.Dx334.Value },
                    PatientDetails = new PatientDetails
                    { CurrentAddress = new Address { PostalCode = dosScenario.Postcode } }
                })
                .BeginAsync();

            var callbackAcceptancePage = NavigateToRemappedEDOutcome(dosScenario.Postcode);
            callbackAcceptancePage.VerifyIsCallbackAcceptancePage();
            var personalDetailsPage = callbackAcceptancePage.AcceptCallback();
            personalDetailsPage.VerifyIsPersonalDetailsPage();

            Assert.Fail();
            //var referralConfirmation =
            //    personalDetailsPage.SubmitPersonalDetails("Test", "Tester", "02380555555", "01", "01", "1982");
            //referralConfirmation.VerifyIsServiceUnavailableReferral();
            //SaveScreenAsPNG("ed-reval-unavailable-referral");

            //var dosResult = await _testBench.Verify(dosScenario);
            //var esbResult = await _testBench.Verify(esbScenario);
        }

        private TestBench _testBench;

        private void AssertReturnedServiceExists(string serviceName)
        {
            Assert.IsTrue(Driver.ElementExists(By.XPath(string.Format("//H3[text()='{0}']", serviceName))));
        }

        private void AssertIsOriginalOutcome(OutcomePage edOutcome, string expectedDispositionCode = "Dx02",
            string dispositionText = "Go to an emergency treatment centre urgently")
        {
            edOutcome.VerifyOutcome(dispositionText);
            edOutcome.VerifyDispositionCode(expectedDispositionCode);
        }

        private void AssertIsPostcodePage(OutcomePage edOutcome)
        {
            edOutcome.VerifyOutcome("Where do you want help?");
        }

        private PersonalDetailsPage ClickBookCallButton(OutcomePage edOutcome)
        {
            Driver.FindElement(By.Name("PersonalDetails")).Click();
            return new PersonalDetailsPage(Driver);
        }

        private OutcomePage NavigateToRemappedEDOutcome(Postcode postcode)
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult, postcode.Value);

            return questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 3)
                .Answer<OutcomePage>("Yes");
        }

        private OutcomePage NavigateToDx94Outcome()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Sexual or Menstrual Concerns",
                TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult);

            return questionPage.Answer<OutcomePage>(1);
        }

        private OutcomePage EnterPostCodeAndSubmit(string postcode)
        {
            var postcodeField = Driver.FindElement(By.Id("CurrentPostcode"));
            postcodeField.Clear();
            postcodeField.SendKeys(postcode);
            Driver.FindElement(By.Id("postcode")).Click();
            return new OutcomePage(Driver);
        }
    }
}
