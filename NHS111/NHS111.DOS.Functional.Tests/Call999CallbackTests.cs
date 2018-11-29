namespace NHS111.DOS.Functional.Tests {
    using System.Diagnostics;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using SmokeTest.Utils;

    public class Call999CallbackTests
    : BaseTests {

        [Test]
        public void Call999Cat3_WithCallbackReturned_DisplaysPersonalPage() {
            var callbackPage = NavigateTo999Cat3("432154ACCF327E1B1981ECC806F6DB26C3509A25B124244621230791B02C1A16C654ABEFAE5F6530CB181FE154D22542D0ECB21E31F3B2FE823BE54F9231B0049CB68E60523855E263AC816174402971");
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

        private string _ls11ns =
            "432154ACCF327E1B78D069DD9241999747769F5BA3F1CE6ACF968DD69054F3993C802DA7E31D9D281C758ECE1B0AC67C8D9D5E4AB1C2F0F4FAFF04B7F8CDC22F700D663341C6CCB1";

        private string _ls11az =
            "432154ACCF327E1B5F710FF8C339035DD91FBB860C9EA164B2CE0C413DA5ECF8256239610A48C783B015F33881F062059D73D281C77B2604EB0010519B0170FCC52BE0888027EE41";

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
