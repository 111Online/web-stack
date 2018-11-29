namespace NHS111.DOS.Functional.Tests {
    using System;
    using System.Diagnostics;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using SmokeTest.Utils;

    public class Call999CallbackTests
    : BaseTests {

        [Test]
        public void Call999Cat3_WithCallbackReturned_DisplaysPersonalPage() {
            var callbackPage = NavigateTo999Cat3(_ls11az);
            AssertIsCallbackAcceptancePage(callbackPage);
            var personalDetailsPage = AcceptCallback();
            AssertIsPersonalDetailsPage(personalDetailsPage);
        }

        [Test]
        public void Call999Cat3_WithoutCallbackReturned_DisplaysOriginalDispo() {
            var outcome = NavigateTo999Cat3(_ls11ns);
            outcome.VerifyOutcome(OutcomePage.Cat3999Text);
        }

        [Test]
        public void Call999Cat4_WithCallbackReturned_DisplaysPersonalDetailsPage() {
            var callbackPage = NavigateTo999Cat4(_ls11az);
            AssertIsCallbackAcceptancePage(callbackPage);
            var personalDetailsPage = AcceptCallback();
            AssertIsPersonalDetailsPage(personalDetailsPage);
        }

        [Test]
        public void Call999Cat4_WithoutCallbackReturned_DisplaysOriginalDispo() {
            var outcome = NavigateTo999Cat4(_ls11ns);
            outcome.VerifyOutcome(OutcomePage.Cat4999Text);
        }

        [Test] //should succeed with and without callback service assigned (it shouldn't return)
        public void Call999Cat2_Never_OffersCallback() {
            var outcomePage = NavigateTo999Cat2();

            outcomePage.VerifyOutcome(OutcomePage.Cat2999Text);
        }

        [Test] //to be run without postcode
        public void Call999Cat3_WithoutPostcode_AsksForPostcode() {
            var postcodePage = NavigateTo999Cat3();
            AssertIsCallbackAcceptancePage(postcodePage);
            Assert.True(Driver.ElementExists(By.Id("FindService_CurrentPostcode")), "Expected postcode field when no gate.");
            var personalDetailsPage = SubmitPostcode("ls11az", postcodePage);
            AssertIsPersonalDetailsPage(personalDetailsPage);
        }

        [Test]
        public void SubmittingReferralForCat3_WhenSuccessful_ShowsConfirmationScreen() {
            var callbackPage = NavigateTo999Cat3(_ls11az);
            AssertIsCallbackAcceptancePage(callbackPage);
            var personalDetailsPage = AcceptCallback();
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsSuccessfulReferral(itkConfirmation);
        }

        [Test]
        public void SubmittingReferralForCat3_WhenUnsuccessful_ShowsFailureScreen() {
            var callbackPage = NavigateTo999Cat3(_ls11rf);
            AssertIsCallbackAcceptancePage(callbackPage);
            var personalDetailsPage = AcceptCallback();
            AssertIsPersonalDetailsPage(personalDetailsPage);
            var itkConfirmation = SubmitPersonalDetails(personalDetailsPage);
            AssertIsUnsuccessfulReferral(itkConfirmation);
        }

        private string _ls11rf =
            "432154ACCF327E1B39501267C7CA6F3244A6D49DE3CA16814284F7AFB21B44A93D4EA67214A24A13DC2BDD1ABF83E9EC80346237D9456BD1BF229020CCA05343A7618B68CFCF8623";

        private string _ls11ns =
            "432154ACCF327E1B78D069DD9241999747769F5BA3F1CE6ACF968DD69054F3993C802DA7E31D9D281C758ECE1B0AC67C8D9D5E4AB1C2F0F4FAFF04B7F8CDC22F700D663341C6CCB1";

        private string _ls11az =
            "432154ACCF327E1B5F710FF8C339035DD91FBB860C9EA164B2CE0C413DA5ECF8256239610A48C783B015F33881F062059D73D281C77B2604EB0010519B0170FCC52BE0888027EE41";

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

        private void AssertIsSuccessfulReferral(OutcomePage itkConfirmation)
        {
            Assert.IsTrue(Driver.ElementExists(By.CssSelector("h1")), "Possible unexpected triage outcome. Expected header to exist but it doesn't.");
            var header = Driver.FindElement(By.CssSelector("h1"));
            Assert.IsTrue(header.Text.StartsWith("You should get a call within"), string.Format("Possible unexpected triage outcome. Expected header text of 'You should get a call within' but was '{0}'.", header.Text));
        }

        private void AssertIsUnsuccessfulReferral(OutcomePage itkConfirmation)
        {
            itkConfirmation.VerifyOutcome("Sorry, there is a problem with the service");
        }

        private void AssertIsDuplicateReferral(OutcomePage itkConfirmation)
        {
            itkConfirmation.VerifyOutcome("Your call has already been booked");
        }
        private OutcomePage NavigateTo999Cat4(string args) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Finger or Thumb Injury, Penetrating", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, args);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 5)
                .AnswerForDispostion<OutcomePage>("No");
            return outcomePage;
        }

        private OutcomePage NavigateTo999Cat3(string args = null) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, args);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");
            return outcomePage;
        }

        private OutcomePage NavigateTo999Cat2() {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Breathing Problems, Breathlessness or Wheeze",
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

    }
}
