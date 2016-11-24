
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
                //_driver.Quit();
                //_driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

       

        [Test]
        public void Call999EndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Skin problems", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("What is the main problem?");
            var outcomePage =  questionPage
                .Answer("A rash")
                .AnswerSuccessiveYes(2)
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }

        [Test]
        public void PharmacyEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Eye problems", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer("Eye redness or irritation")
                .AnswerSuccessiveNo(2)
                .Answer("My problem affects one eye")
                .AnswerSuccessiveNo(4)
                .Answer("No")
                .AnswerNo()
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should contact a pharmacist within 12 hours");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyFindService(FindServiceTypes.Pharmacy);
            outcomePage.VerifyCareAdviceHeader("What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new[] { "Eye discharge" });
        }

        [Test]
        public void HomeCareEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Colds and flu", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Are you severely ill AND got new marks, like bruising or bleeding under the skin?");
            var outcomePage = questionPage
                .AnswerSuccessiveNo(6)
                .Answer("None of the above")
                .Answer("No - I don't have diabetes")
                .AnswerForDispostion("No");


            outcomePage.VerifyOutcome("Based on your answers, you can look after yourself and don't need to see a healthcare professional");
           // outcomePage.VerifyHeaderOtherInfo("Based on your answers you do not need to see a healthcare profesional at this time.\r\nPlease see the advice below on how to look after yourself");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            
        }

        [Test]
        public void DentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Dental problems", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer("Toothache")
                .AnswerSuccessiveNo(8)
                .Answer("No - less than 14 days")
                .AnswerForDispostion("No - I've not taken any painkillers");

            outcomePage.VerifyOutcome("Your answers suggest you should see a dentist within 5 working days");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyFindService(FindServiceTypes.Dental);
            outcomePage.VerifyCareAdviceHeader("What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new string[] { "Toothache", "Medication, pain and/or fever" });
        }

        [Test]
        public void EmergencyDentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Dental problems", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer("Dental bleeding")
                .AnswerNo()
                .AnswerYes()
                .Answer("A tooth extraction")
                .AnswerNo()
                .AnswerForDispostion("It's getting worse");


            outcomePage.VerifyOutcome("Your answers suggest you need urgent attention for your dental problem within 4 hours");
            outcomePage.VerifyFindService(FindServiceTypes.EmergencyDental);
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyCareAdviceHeader("What can I do in the meantime?");
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

            outcomePage.VerifyOutcome("Your answers suggest you need urgent medical attention within 1 hour");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call999);
            outcomePage.VerifyFindService(FindServiceTypes.AccidentAndEmergency);
            outcomePage.VerifyCareAdviceHeader("What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new string[] { "Headache", "Breathlessness", "Medication, pain and/or fever" });
        }

        [Test]
        public void OpticianEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Eye problems", TestScenerioGender.Female, TestScenerioAgeGroups.Child);

            questionPage.ValidateQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer("A sticky or watery discharge from your eye")
                .AnswerNo()
                .AnswerYes()
                .AnswerSuccessiveNo(2)
                .Answer("My problem affects both eyes")
                .AnswerSuccessiveNo(9) 
                .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("Your answers suggest you should see an optician within 3 days");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyFindService(FindServiceTypes.Optician);
            outcomePage.VerifyCareAdviceHeader("What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new string[] {"Eye discharge", "Medication, pain and/or fever" });
        }

        [Test]
        public void GPEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Diarrhoea and vomiting", TestScenerioGender.Male, TestScenerioAgeGroups.Child);

            questionPage.ValidateQuestion("Have you had any blood in your sick (vomit)?");
            var outcomePage = questionPage
                .AnswerSuccessiveNo(4)
                .Answer("None of these")
                .AnswerSuccessiveNo(2)
                .Answer("No - I don't have diabetes")
                .AnswerNo()
                .Answer("None of these")
                .AnswerSuccessiveNo(8)
                .AnswerForDispostion("Yes - 1 week or more");

            outcomePage.VerifyOutcome("Your answers suggest you should speak to your GP within 3 days");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyCareAdviceHeader("What can I do in the meantime?");
            outcomePage.VerifyCareAdvice(new string[] { "Diarrhoea & Vomiting" });
        }






    }
}
