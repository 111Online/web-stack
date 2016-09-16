
using System;
using System.Text;
using NHS111.SmokeTest.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class EndpointJourneys
    {
        private IWebDriver _driver;

        [TestFixtureSetUp]
        public void InitTests()
        {
            _driver = new ChromeDriver();
        }

        [TestFixtureTearDown]
        public void TeardownTest()
        {
            try
            {
                _driver.Quit();
                _driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

       

        [Test]
        public void Call999EndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Skin, Rash", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Is your rash made up of blisters?");
            var outcomePage =  questionPage
                .AnswerSuccessiveYes(2)
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Call 999");
        }

        [Test]
        public void PharmacyEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Eye, Red or Irritable", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Have you noticed any new changes to your vision?");
            var outcomePage = questionPage
                .AnswerSuccessiveNo(2)
                .Answer("My problem affects one eye")
                .AnswerSuccessiveNo(4)
                .Answer("No, less than two weeks")
                .AnswerNo()
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should see a pharmacist within 12 hours");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyFindService(FindServiceTypes.Pharmacy);
            outcomePage.VerifyCareAdviceHeader("I know which pharmacy I'm going to. What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new[] { "Eye discharge" });
        }

        [Test]
        public void HomeCareEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Cold or Flu (Declared)", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Are you feeling severely ill and you also have new marks under your skin that look like bruising or bleeding?");
            var outcomePage = questionPage
                .AnswerSuccessiveNo(6)
                .Answer("None of the above")
                .AnswerNo()
                .AnswerForDispostion("No, less than two weeks");


            outcomePage.VerifyOutcome("You've finished your online assessment");
            outcomePage.VerifySubHeader("What Happens next?");
            outcomePage.VerifyHeaderOtherInfo("Based on your answers you do not need to see a healthcare profesional at this time.\r\nPlease see the advice below on how to look after yourself");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            
        }

        [Test]
        public void EmergencyDentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Dental Bleeding", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Was the bleeding caused by an injury?");
            var outcomePage = questionPage
                .AnswerNo()
                .AnswerYes()
                .Answer("A tooth extraction")
                .AnswerNo()
                .AnswerForDispostion("It's getting worse");


            outcomePage.VerifyOutcome("Your answers suggest you should get emergency dental treatment within 4 hours");
            outcomePage.VerifyFindService(FindServiceTypes.EmergencyDental);
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call999);
            outcomePage.VerifyCareAdviceHeader("I know where to get emergency dental treatment. What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new string[] { "Tooth extraction" });
        }

        [Test]
        public void AccidentAndEmergencyEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Headache", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Have you had an injury to your head in the last 7 days?");
            var outcomePage = questionPage
                .AnswerSuccessiveNo(8)
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should go to A&E within 1 hour");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call999);
            outcomePage.VerifyFindService(FindServiceTypes.AccidentAndEmergency);
            outcomePage.VerifyCareAdviceHeader("I know which A&E I'm going to. What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new string[] { "Headache", "Breathlessness", "Medication, pain and/or fever" });
        }

        [Test]
        public void OpticianEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Eye, Sticky, Watery", TestScenerioGender.Female, TestScenerioAgeGroups.Child);

            questionPage.ValidateQuestion("Have you noticed any new changes to your vision?");
            var outcomePage = questionPage
                .AnswerNo()
                .AnswerYes()
                .AnswerSuccessiveNo(2)
                .Answer("My problem affects both eyes")
                .AnswerSuccessiveNo(6)
                .Answer("No, less than two weeks")
                .AnswerSuccessiveNo(2)
                .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("Your answers suggest you should see an optician within 3 days");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyFindService(FindServiceTypes.Optician);
            outcomePage.VerifyCareAdviceHeader("I know which optician I'm going to. What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new string[] { "Medication, pain and/or fever" });
        }







    }
}
