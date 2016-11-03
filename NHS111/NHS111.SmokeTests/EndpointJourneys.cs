
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
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Skin Problems", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("What is your main problem?");
            var outcomePage =  questionPage
                .Answer("You have a rash")
                .AnswerSuccessiveYes(2)
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Call 999");
        }

        [Test]
        public void PharmacyEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Eye or Eyelid Problems", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("What is your main problem?");
            var outcomePage = questionPage
                .Answer("You've redness or irritation of your eye")
                .AnswerSuccessiveNo(2)
                .Answer("My problem affects one eye")
                .AnswerSuccessiveNo(4)
                .Answer("No")
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

            questionPage.ValidateQuestion("Do you have new bruises, a rash, or marks on your skin and feel severely ill?");
            var outcomePage = questionPage
                .AnswerSuccessiveNo(6)
                .Answer("None of the above")
                .Answer("No - I don't have diabetes")
                .AnswerForDispostion("No");


            outcomePage.VerifyOutcome("Based on your answers you do not need to see a healthcare professional at this time. Please see the advice below on how to look after yourself");
            outcomePage.VerifySubHeader("You've finished your online assessment");
           // outcomePage.VerifyHeaderOtherInfo("Based on your answers you do not need to see a healthcare profesional at this time.\r\nPlease see the advice below on how to look after yourself");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            
        }

        [Test]
        public void EmergencyDentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Dental Problems", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Which of the following apply to you?");
            var outcomePage = questionPage
                .Answer("My teeth or gums are bleeding")
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

            questionPage.ValidateQuestion("Have you had a bang on the head or injured your head in the last 7 days?");
            var outcomePage = questionPage
                .AnswerSuccessiveNo(7)
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
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Eye or Eyelid Problems", TestScenerioGender.Female, TestScenerioAgeGroups.Child);

            questionPage.ValidateQuestion("What is your main problem?");
            var outcomePage = questionPage
                .Answer("You've a sticky or watery eye discharge")
                .AnswerNo()
                .AnswerYes()
                .AnswerSuccessiveNo(2)
                .Answer("My problem affects both eyes")
                .AnswerSuccessiveNo(9) 
                .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("Your answers suggest you should see an optician within 3 days");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyFindService(FindServiceTypes.Optician);
            outcomePage.VerifyCareAdviceHeader("I know which optician I'm going to. What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new string[] {"Eye discharge", "Medication, pain and/or fever" });
        }

        [Test]
        public void GPEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Diarrhoea and Vomiting", TestScenerioGender.Male, TestScenerioAgeGroups.Child);

            questionPage.ValidateQuestion("Have you had any blood in your sick (vomit)?");
            var outcomePage = questionPage
                .AnswerSuccessiveNo(4)
                .Answer("None of these")
                .AnswerSuccessiveNo(2)
                .Answer("No - I don't have diabetes")
                .AnswerSuccessiveNo(10)
                .AnswerForDispostion("Yes (one week or more)");

            outcomePage.VerifyOutcome("Your answers suggest you should speak to your GP");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyCareAdviceHeader("What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new string[] { "Diarrhoea & Vomiting" });
        }






    }
}
