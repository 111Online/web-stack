
namespace NHS111.DOS.Functional.Tests {
    using System;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using SmokeTest.Utils;

    /// Tests the callback/validation flow for Emergency Department outcomes.
    public class EDValidationTests
        : BaseTests {

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
            var callbackAcceptancePage = EnterPostcode("CA2 7HY", postcodePage);
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
            AssertIsSuccessfulITK(itkConfirmation);
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

        private void AssertReturnedServiceExists(string serviceName) {
            Assert.IsTrue(Driver.ElementExists(By.XPath($"//H3[text()='{serviceName}']")));
        }


        private void AssertIsSuccessfulITK(OutcomePage itkConfirmation) {
            itkConfirmation.VerifyOutcome("Your call is confirmed");
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

        private string _ls167nz = 
            "432154ACCF327E1B2F6D2D6D5E30F42F2C250BCF6C6498AC8A5889976FB129C03298B5E56A926CA17093D3116C20348C56D773CE5ABC419C3A5B60CFFA1A69168328C4D32793E5BC18B61B22FC5F9519";

        private string _ca37hy =
            "432154ACCF327E1BBBE4CD01D891D61D8E8A7896653ED58FE8E00DF2F3D79E55D04A5A6F6C71EA977A2CF2A21076A9ADF1E0FBCF095603A706CCD53D2A1DF76A1CF0B43BA73BFCF5";

        private string _ca27hy =
            "432154ACCF327E1BEB9C06709738AC05CF21F961B9A8637A4AA8A32E46EE8AA81B6C591DCDF4E06AEEDF77B8C08B67BBC40395BFD2AFDAEF02E12AD8616E8311C03097D516315F00";

        private string _ls197nz =
            "432154ACCF327E1BAD3D41A2A33FFD27BF71802066E4B038446D6AE7DB1B2CF019BD92872FFC584271506172BC3D48C64B249CB8A3216AFC7401FF048DAEA38C0C5CBFF9CA58E64021801E5090C2E2C9";

        private string _ls187nz =
            "432154ACCF327E1BA624A4940E239D6AE97BB59A5AAD2D51BF2EDD7C9B0A9247849B3D0FF55AA5FA8178A747609968A7783D3DBA645291D8AFF628DC8C4C6D160B4A72665FA896B508AEEE209CBA949A";

        private string _ls177nz =
            "432154ACCF327E1B1981ECC806F6DB26C3509A25B124244621230791B02C1A16C654ABEFAE5F6530CB181FE154D22542D0ECB21E31F3B2FE823BE54F9231B0049CB68E60523855E263AC816174402971";


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
            edOutcome.VerifyOutcome("Get a phone call from a nurse");
        }
    }
}