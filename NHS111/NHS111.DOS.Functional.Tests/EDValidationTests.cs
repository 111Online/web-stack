namespace NHS111.Dos.TestBench.Api {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.Models.Business;
    using Models.Models.Domain;
    using Models.Models.Web.DosRequests;

        public class DosTestScenario
        {
            public string Postcode { get; set; }
            public string Description { get; set; }

            public List<DosTestScenarioRequest> Requests { get; set; }

            public bool Matches(Postcode postcode)
            {
                return new Postcode(this.Postcode).Equals(postcode);
            }

            public DosTestScenario() {
                Requests = new List<DosTestScenarioRequest>();
            }
        }

        public class DosTestScenarioRequest
        {
            public DosFilteredCase InboundDosFilteredCase { get; set; }
            public DosFilteredCase OutboundDosFilteredCase { get; set; }
            public List<IDosTestScenarioTransformer> MatchSequence { get; set; }

            /// <summary>
            /// A mismatch is when the inbound postcode matches but the rest of the InboundDosFilteredCase does not.
            /// </summary>
            public List<IDosTestScenarioTransformer> MismatchSequence { get; set; }

            public DosTestScenarioRequest() {
                MatchSequence = new List<IDosTestScenarioTransformer>();
                MismatchSequence = new List<IDosTestScenarioTransformer>();
            }
        }

    public interface IDosTestScenarioTransformer {
        string Name { get; }
        object[] Arguments { get; }
    }
    public class DosTestScenarioTransformer
        {
            public string Name { get; set; }
            public object[] Arguments { get; set; }
        }

    public interface IDosScenarioSetup {
        IDosCallSetup ExpectingCall(ToEndpoint.IDosEndpoint endpoint);
    }


    public interface IDosCallSetup {
        IDosCallSetup Matching(DosFilteredCase dosCase);
        IDosCallSetup Returns(params IDosTestScenarioTransformer[] transformers);
        IDosCallSetup OtherwiseReturns(params IDosTestScenarioTransformer[] mismatchTransformers);
        IDosScenarioSetup Then();
        Postcode Begin();
    }

    public class DosTestBenchSetup

        : IDosScenarioSetup, IDosCallSetup
    {
        public IDosCallSetup ExpectingCall(ToEndpoint.IDosEndpoint endpoint) {
            _currentRequest = new DosTestScenarioRequest();
            return this;
        }

        public IDosCallSetup Matching(DosFilteredCase dosCase) {
            _currentRequest.InboundDosFilteredCase = dosCase;
            return this;
        }

        public IDosCallSetup Returns(params IDosTestScenarioTransformer[] transformers) {
            _currentRequest.MatchSequence.AddRange(transformers);
            return this;
        }

        public IDosCallSetup OtherwiseReturns(params IDosTestScenarioTransformer[] mismatchTransformers) {
            _currentRequest.MismatchSequence.AddRange(mismatchTransformers);
            return this;
        }

        public IDosScenarioSetup Then() {
            _scenario.Requests.Add(_currentRequest);
            _currentRequest = null;
            return this;
        }

        public Postcode Begin() {
            throw new NotImplementedException();
        }

        private DosTestScenarioRequest _currentRequest = null;
        private DosTestScenario _scenario = new DosTestScenario();
    }

    public class TestBenchApi {
        public IDosScenarioSetup SetupDosScenario() { return new DosTestBenchSetup(); }

        public void Verify(Postcode scenarioPostcode) {
            throw new NotImplementedException();
        }
    }

    public static class For {
        public static DosFilteredCase DxCode(DispositionCode dispositionCode) {
            return new DosFilteredCase {
                Disposition = dispositionCode.DosCode
            };
        }
    }

    public interface IServiceListTransformer { }

    public static class ToEndpoint {
        public interface IDosEndpoint { }

        public static IDosEndpoint CheckCapacitySummary { get; set; }
    }

    public static class DosCase {
        public static DosFilteredCase WithDxCode(DispositionCode dispositionCode) {
            return new DosFilteredCase {
                Disposition = dispositionCode.DosCode
            };
        }
    }

    public interface IDosCallMismatchResult
    {

    }

    public static class DosCallMismatchResult
    {
        public static IDosTestScenarioTransformer ServerError { get; set; }
    }

    public static class ServicesTransformedTo {
        public static IDosTestScenarioTransformer AtLeastOneCallback { get; set; }
    }

}

namespace NHS111.DOS.Functional.Tests {
    using System;
    using Dos.TestBench.Api;
    using Models.Models.Domain;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using SmokeTest.Utils;


    /// Tests the callback/validation flow for Emergency Department outcomes.
    public class EDValidationTests
        : BaseTests {

        [SetUp]
        public void Setup() {
            _testBench = new TestBenchApi();
        }

        [Test]
        public void EDOutcome_MappedToDx334ButNoCallbackServicesReturned_ShowsOriginalOutcome() {
            var edOutcome = NavigateToRemappedEDOutcome(_ca37hy);
            AssertIsOriginalOutcome(edOutcome);
        }

        [Test]
        public void EDOutcome_MappedToDx334AndReturnsCallbackServices_ShowsCallbackAcceptancePage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_ca27hy);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
        }

        [Test]
        public void EDOutcome_WhenUserRejectsCallbackOffer_ShowsOriginalOutcome() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_ca27hy);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var edOutcome = RejectCallback(callbackAcceptancePage);
            AssertIsOriginalOutcome(edOutcome);
        }

        [Test]
        public void EDOutcome_WhenUserAcceptsCallbackOffer_ShowsPersonalDetailsPage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_ca27hy);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var edOutcome = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(edOutcome);
        }

        [Test] //no postcode present
        public void EDOutcome_ThenEnteringPostcodeReturningCallback_ShowsCallbackThenPersonalDetailsPage() {
            var postcodePage = NavigateToRemappedEDOutcome();
            AssertIsPostcodePage(postcodePage);
            var callbackAcceptancePage = EnterPostcode("CA27HY", postcodePage);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var edOutcome = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(edOutcome);
        }

        [Test]
        public void Dx94_WithNoCallbackServices_ShowOriginalOutcome() {
            var edOutcome = NavigateToDx94Outcome(_ls197nz);
            AssertIsOriginalOutcome(edOutcome, "Dx94", "Your answers suggest you should go to A&E within 1 hour");
        }

        [Test]
        public void AcceptingCallbackOffer_AfterRejectingDx334Callback_QueriesDosWithCorrectDxCodes()
        {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_ls177nz);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var edOutcome = RejectCallback(callbackAcceptancePage);
            AssertIsOriginalOutcome(edOutcome);
            var personalDetailsPage = ClickBookCallButton(edOutcome);
            AssertIsPersonalDetailsPage(personalDetailsPage);
        }

        [Test]
        public void SubmittingReferralRequest_AfterRejectingDx334Callback_SubmitsReferralWithCorrectDxCode() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_ls177nz);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var edOutcome = RejectCallback(callbackAcceptancePage);
            AssertIsOriginalOutcome(edOutcome);
            var personalDetailsPage = ClickBookCallButton(edOutcome);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsSuccessfulReferral(itkConfirmation);
        }

        [Test]
        public void SubmittingReferralRequestForDx02Callback_WhenNoDx334Callbacks_SubmitsReferralWithCorrectDxCode()
        {
            var edOutcome = NavigateToRemappedEDOutcome(_ls117nz);
            AssertIsOriginalOutcome(edOutcome);
            var personalDetailsPage = ClickBookCallButton(edOutcome);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsSuccessfulReferral(itkConfirmation);
        }


        [Test]
        public void EDOutcome_CallbacksOnlyForDx02_ShowsOriginalOutcomeWithCallbackOffer() {
            var edOutcome = NavigateToRemappedEDOutcome(_ls187nz);
            AssertIsOriginalOutcome(edOutcome);
        }

        [Test]
        public void EDOutcome_WithDosErrorForFirstQuery_ReturnsResultsForSecondQuery()
        {
            var edOutcome = NavigateToRemappedEDOutcome(_ls167nz);
            AssertIsOriginalOutcome(edOutcome);
            AssertReturnedServiceExists("Test service returned from second lookup");
        }

        [Test]
        public void SubmittingReferralRequest_WithSuccessfulReferral_ShowConfirmationPage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_ls177nz);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var personalDetailsPage = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsSuccessfulReferral(itkConfirmation);
            SaveScreenAsPNG("ed-reval-successful-referral");
        }

        [Test]
        public void SubmittingReferralRequest_WithUnsuccessfulReferral_ShowUnsuccessfulPage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_ls176nz);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var personalDetailsPage = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsUnsuccessfulReferral(itkConfirmation);
            SaveScreenAsPNG("ed-reval-unsuccessful-referral");
        } 

        [Test]
        public void SubmittingReferralRequest_WithDuplicateReferral_ShowDuplicatePage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_ls175nz);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var personalDetailsPage = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsDuplicateReferral(itkConfirmation);
            SaveScreenAsPNG("ed-reval-duplicate-referral");
        }

        [Test]
        public void SubmittingReferralRequest_WithUnavailableService_ShowServiceUnavailablePage() {
            var callbackAcceptancePage = NavigateToRemappedEDOutcome(_ls178nz);
            AssertIsCallbackAcceptancePage(callbackAcceptancePage);
            var personalDetailsPage = AcceptCallback(callbackAcceptancePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsServiceUnavailableReferral(itkConfirmation);
            SaveScreenAsPNG("ed-reval-unavailable-referral");
        }

        private void AssertReturnedServiceExists(string serviceName) {
            Assert.IsTrue(Driver.ElementExists(By.XPath(string.Format("//H3[text()='{0}']", serviceName))));
        }

        private void AssertIsSuccessfulReferral(OutcomePage itkConfirmation) {
            Assert.IsTrue(Driver.ElementExists(By.CssSelector("h1")), "Possible unexpected triage outcome. Expected header to exist but it doesn't.");
            var header = Driver.FindElement(By.CssSelector("h1"));
            Assert.IsTrue(header.Text.StartsWith("You should get a call within"), string.Format("Possible unexpected triage outcome. Expected header text of 'You should get a call within' but was '{0}'.", header.Text));
        }

        private void AssertIsUnsuccessfulReferral(OutcomePage itkConfirmation) {
            itkConfirmation.VerifyOutcome("Sorry, there's a problem with the service");
        }

        private void AssertIsDuplicateReferral(OutcomePage itkConfirmation) {
            itkConfirmation.VerifyOutcome("Your call has already been booked");
        }

        private void AssertIsServiceUnavailableReferral(OutcomePage itkConfirmation) {
            //itkConfirmation.VerifyOutcome("Your call has already been booked");
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

        private string _ls178nz =
            "432154ACCF327E1B33F2CAF16477B5DE51F21E9EE0FC493336004EBCC4A6F79A5BDEE2A693EA50B53575D0236420A6C8FCBA09869C8DC938AC89A3167330D11DCA7827BC6FDB657F3AADC6E0AC1811F8";

        private string _ls175nz =
            "432154ACCF327E1B9CCAE6B735F499354EA8958F65B25EC02592817F14E0A774A05FFA8BF5C38180064F61ED1C62CF507B70F20486FC370F1B130DC659C5EDE81BBF58C9E930EFBB82358AA273D3994E";

        private string _ls176nz =
            "432154ACCF327E1BB5660DC198F768436E2338AE511174BA5D5FA142FAA872FD019DC7B4A0A126612AECBC82241FFD0C740434C61A8CE64A5B8387909D0175AC8DCF26D31F79C9D0B51B0FC52E9801E4";

        private string _ls167nz = 
            "432154ACCF327E1B2F6D2D6D5E30F42F2C250BCF6C6498AC8A5889976FB129C03298B5E56A926CA17093D3116C20348C56D773CE5ABC419C3A5B60CFFA1A69168328C4D32793E5BC18B61B22FC5F9519";

        private string _ca37hy =
            "432154ACCF327E1BBBE4CD01D891D61D8E8A7896653ED58FE8E00DF2F3D79E55D04A5A6F6C71EA977A2CF2A21076A9ADF1E0FBCF095603A706CCD53D2A1DF76A1CF0B43BA73BFCF5";

        private string _ca27hy =
            "432154ACCF327E1BEB9C06709738AC05CF21F961B9A8637A4AA8A32E46EE8AA81B6C591DCDF4E06AEEDF77B8C08B67BBC40395BFD2AFDAEF02E12AD8616E8311C03097D516315F00";

        private string _ls197nz =
            "432154ACCF327E1BAD3D41A2A33FFD27BF71802066E4B038446D6AE7DB1B2CF019BD92872FFC584271506172BC3D48C64B249CB8A3216AFC7401FF048DAEA38C0C5CBFF9CA58E64021801E5090C2E2C9";

        private string _ls117nz =
            "432154ACCF327E1B379AADDE14BE71A9AA81D59812F9D89161CA89EA4A905477BD196D61FF7E9542A9946AA21C614AF72790CCA95B5D35C9FDD1E38424E74B8DB2B4806242427365CB9D866A060F718E";

        private string _ls187nz =
            "432154ACCF327E1BA624A4940E239D6AE97BB59A5AAD2D51BF2EDD7C9B0A9247849B3D0FF55AA5FA8178A747609968A7783D3DBA645291D8AFF628DC8C4C6D160B4A72665FA896B508AEEE209CBA949A";

        private string _ls177nz =
            "432154ACCF327E1B1981ECC806F6DB26C3509A25B124244621230791B02C1A16C654ABEFAE5F6530CB181FE154D22542D0ECB21E31F3B2FE823BE54F9231B0049CB68E60523855E263AC816174402971";

        private TestBenchApi _testBench;


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
    }
}