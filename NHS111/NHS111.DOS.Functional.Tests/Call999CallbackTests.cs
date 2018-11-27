namespace NHS111.DOS.Functional.Tests {
    using System.Diagnostics;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using SmokeTest.Utils;

    public class Call999CallbackTests
    : BaseTests {

        //[TestCase(OutcomePage.Cat3999Text)]  //no callback returned
        [TestCase(OutcomePage.Call999CallbackText)] //callback returned
        public void Call999Cat3_WithDosResult_DisplaysExpectedDispositionPage(string expectedOutcomeText) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome(expectedOutcomeText);
        }

        //[TestCase(OutcomePage.Cat4999Text)] //no callback returned
        [TestCase(OutcomePage.Call999CallbackText)] //callback returned
        public void Call999Cat4_WithDosResult_DisplaysExpectedDispositionPage(string expectedOutcomeText)
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Finger or Thumb Injury, Penetrating", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 5)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome(expectedOutcomeText);
        }

        [Test] //should succeed with and without callback service assigned (it shouldn't return)
        public void Call999Cat2_Never_OffersCallback()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Breathing Problems, Breathlessness or Wheeze", TestScenerioSex.Male, TestScenerioAgeGroups.Child);

            var outcomePage = questionPage
                .Answer(3)
                .Answer(1)
                .Answer(4)
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome(OutcomePage.Cat2999Text);
        }

        [Test] //to be run without postcode
        public void Cat3and4Call999_WithoutPostcode_AsksForPostcode()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .Answer(1)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse needs to phone you");
            Assert.True(Driver.ElementExists(By.Id("FindService_CurrentPostcode")), "Expected postcode field when no gate.");
        }

        private OutcomePage SubmitPostcode(string postcode, OutcomePage postcodePage)
        {
            Assert.True(Driver.ElementExists(By.Id("FindService_CurrentPostcode")), "Postcode field not available");
            Driver.FindElement(By.Id("FindService_CurrentPostcode")).Clear();
            Driver.FindElement(By.Id("FindService_CurrentPostcode")).SendKeys(postcode);
            Driver.FindElement(By.Id("DosLookup")).Click();
            return new OutcomePage(Driver);
        }


    }
}
