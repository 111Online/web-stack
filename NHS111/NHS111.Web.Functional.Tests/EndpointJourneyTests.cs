using NHS111.Web.Functional.Utils;
using NHS111.Web.Functional.Utils.ScreenShot;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using OpenQA.Selenium;
    
    [TestFixture]
    [ScreenShotComparison]
    public class EndpointJourneyTests : BaseTests {

        [TestCase("Male", 22, "Headache", new[] {3, 3, 3, 5, 3, 3, 3, 1}, "Dx02", TestName = "Can reach Dx02")]
        [TestCase("Male", 24, "Sexual Concerns", new[] {3, 4, 3, 3, 3, 4, 4, 1, 1, 3}, "Dx03", TestName = "Can reach Dx03")]
        [TestCase("Female", 24, "Sexual or Menstrual Concerns", new[] {3, 4}, "Dx38", TestName = "Can reach Dx38")]
        [TestCase("Male", 6, "Object, Ingested or Inhaled", new[] {1, 3, 3, 5, 3, 5, 3, 3, 3, 3, 3, 3}, "Dx89", TestName = "Can reach Dx89")]
        [TestCase("Female", 16, "Mental Health Problems", new[] {1, 5, 3, 5, 3, 1, 4}, "Dx92", TestName = "Can reach Dx92")]
        [TestCase("Female", 22, "Sexual or Menstrual Concerns", new[] {1}, "Dx94", TestName = "Can reach Dx94")]
        public void TestOutcomes(string sex, int age, string pathwayTitle, int[] answers, string expectedDxCode) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, pathwayTitle, sex, age);

            for (var i = 0; i < answers.Length - 1; i++) {
                questionPage.Answer(answers[i]);
            }

            var outcomePage = questionPage
                .Answer<OutcomePage>(answers.Last());

            outcomePage.VerifyDispositionCode(expectedDxCode);
            outcomePage.CompareAndVerify("1");
        }

        [Test]
        public void Call999EndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Skin Problems", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer(1)
                .AnswerSuccessiveByOrder(1, 2)
                .Answer<OutcomePage>(1);

            outcomePage.VerifyOutcome("Phone 999 now for an ambulance");
            outcomePage.CompareAndVerify(outcomePage, "1");
        }

        [Test]
        public void Call999EndpointJourneyCAT1()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Skin Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .AnswerSuccessiveByOrder(1, 2)
                .Answer<OutcomePage>(1);

            outcomePage.VerifyOutcome("Phone 999 now for an ambulance");
            outcomePage.CompareAndVerify(outcomePage, "1");
        }

        [Test]
        public void PharmacyEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Eye or Eyelid Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .AnswerSuccessiveByOrder(1, 1)
                .AnswerSuccessiveByOrder(3, 6)
                .Answer<OutcomePage>(1);
 
            outcomePage.VerifyOutcome("Your answers suggest you should contact a pharmacist within 24 hours");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyFindService(FindServiceTypes.Pharmacy);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new[] { "Eye discharge" });
            outcomePage.CompareAndVerify("1");
        }

        [Test]
        public void HomeCareEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Cold or Flu (Declared)", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Are you so ill that you've stopped doing all of your usual daily activities?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(3) //mers
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .Answer(4)
                .Answer(6)
                .Answer(3)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyOutcome("It's safe to look after yourself");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.CompareAndVerify("1");
        }

        [Test]
        public void DentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("What is the main problem today?");
            var postcodeFirstPage = questionPage
                .Answer(2)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 5)
                .Answer<PostcodeFirstPage>(3);

            postcodeFirstPage.VerifyOutcome("See your dentist in the next few days");
            postcodeFirstPage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            postcodeFirstPage.VerifyCareAdviceHeader("What you can do in the meantime");
            postcodeFirstPage.VerifyCareAdvice(new string[] { "Toothache", "Medication, pain and/or fever" });
            postcodeFirstPage.CompareAndVerify("1");
        }

        [Test]
        public void EmergencyDentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("What is the main problem today?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .Answer(1)
                .Answer(4)
                .Answer<OutcomePage>(1);

            outcomePage.VerifyOutcome("Your answers suggest you need urgent attention for your dental problem within 4 hours");
            outcomePage.VerifyFindService(FindServiceTypes.EmergencyDental);
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Tooth extraction" });
            outcomePage.CompareAndVerify("1");
        }

        [Test]
        public void AccidentAndEmergencyEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Have you hurt or banged your head in the last 4 weeks?");
            var outcomePage = questionPage
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(5)
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .Answer<OutcomePage>(1);

            if (outcomePage.IsCallbackAcceptancePage())
                outcomePage.RejectCallback();
            outcomePage.VerifyOutcome("Go to an emergency treatment centre urgently");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Headache", "Breathlessness", "Medication, pain and/or fever" });
            outcomePage.CompareAndVerify("1");
        }

        [Test]
        public void OpticianEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Eye or Eyelid Problems", TestScenerioSex.Female, TestScenerioAgeGroups.Child);

            questionPage.VerifyQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer(5)
                .Answer(3)
                .Answer(1)
                .Answer(3)
                .Answer(3)
                .Answer(2)
                .Answer(4)
                .Answer(3)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 3)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyOutcome("Your answers suggest you should see an optician within 3 days");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyFindService(FindServiceTypes.Optician);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] {"Eye discharge", "Medication, pain and/or fever" });
            outcomePage.CompareAndVerify("1");
        }

        [Test]
        public void GPEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Diarrhoea and Vomiting", TestScenerioSex.Male, TestScenerioAgeGroups.Child);

            questionPage.VerifyQuestion("Do any of these apply to your sick?");
            var outcomePage = questionPage
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 5)
                .AnswerSuccessiveByOrder(4, 2)
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(3)
                .Answer<OutcomePage>(1);

            outcomePage.VerifyOutcome("Contact your GP if you don't feel better in a few days");
            outcomePage.VerifyWorseningReveal(WorseningMessages.PrimaryCare);
            outcomePage.VerifyCareAdviceHeader("Things to look out for and self-care");
            outcomePage.VerifyCareAdvice(new string[] { "Diarrhoea & Vomiting" });
            outcomePage.CompareAndVerify("1");
        }

        [Test]
        public void JumpToRemappedMentalHealthPathway()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Tiredness (Fatigue)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Have you got a fever right now or had one since the tiredness started?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerSuccessiveByOrder(4, 2)
                .Answer(2)
                .Answer(3)
                .Answer(2)
                .AnswerSuccessiveByOrder(5, 2)
                .AnswerSuccessiveByOrder(2, 4)
                .Answer(3)
                .Answer(1)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyOutcome(OutcomePage.BookCallBackText);
            outcomePage.CompareAndVerify("1");
        }

        [Test]
        public void GPOOHEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Have you hurt or banged your head in the last 4 weeks?");
            var outcomePage = questionPage
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(5)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 4)
                .Answer<PostcodeFirstPage>(1);

            outcomePage.VerifyOutcome("Contact your GP now");
            outcomePage.VerifyWorseningReveal(WorseningMessages.PrimaryCare);
            outcomePage.VerifyCareAdviceHeader("Things to look out for and self-care");
            outcomePage.VerifyCareAdvice(new string[] { "Medication, pain and/or fever", "Headache" });
            outcomePage.CompareAndVerify(outcomePage, "1");
        }

        [Test]
        public void MidwifeEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Is there a chance you're pregnant?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(1)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(5)
                .Answer(3)
                .Answer(3)
                .AnswerSuccessiveByOrder(3, 4)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyFindService(FindServiceTypes.Midwife);
            outcomePage.VerifyOutcome("Your answers suggest you should speak to your midwife within 1 hour");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Medication, pain and/or fever", "Headache" });
            outcomePage.CompareAndVerify("1");
        }

        [Test]
        public void DeadEndJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Trauma Blisters", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            
            var outcomePage = questionPage
                .Answer<DeadEndPage>(1);

            outcomePage.CompareAndVerify("1");
            outcomePage.VerifyOutcome("This health assessment can't be completed online");
        }

        [Test]
        public void GPEndpointJourneyViaDeadEndJump()
        {
            // This test checks that going to a disposition via a dead end jump
            // doesn't break the POST data. That has been a regression found in the past.
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Trauma Blisters", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);
            
            var deadEndPage = questionPage
                .Answer<DeadEndPage>(1);

            // Got to the dead end jump
            deadEndPage.VerifyOutcome("This health assessment can't be completed online");

            questionPage = deadEndPage.ClickPrevious();
            var outcomePage = questionPage.Answer(3)
                .Answer(3)
                .Answer(3)
                .Answer<OutcomePage>(1);

            var otherServicesPage = outcomePage.ClickCantGetAppointment();
            otherServicesPage.VerifyPageContainsDOSServices();
            otherServicesPage.CompareAndVerify("1");
        }

        
        [Test]
        public void EDEndpointJourneyViaPathwayNotFound()
        {
            // This test checks that going to a disposition via a pathway not found
            // doesn't break the POST data. That has been a regression found in the past.
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Wound Problems, Plaster Casts, Tubes and Metal Appliances", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);
            
            var pathwayNotFound = questionPage
                .Answer<OutcomePage>(2);
            
            // Got to pathway not found
            pathwayNotFound.VerifyPathwayNotFound();

            questionPage = pathwayNotFound.ClickPrevious();
            var outcomePage = questionPage.Answer(1)
                .Answer(1)
                .Answer(1)
                .Answer(1)
                .Answer(3)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyPageContainsDOSResults();
            outcomePage.CompareAndVerify("1");
        }
        
        [Test]
        public void ExcludedCareAdviceJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Vomiting and/or Nausea", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .Answer(3)
                .Answer(4)
                .Answer(2)
                .Answer(2)
                .Answer(5)
                .Answer(4)
                .Answer(3)
                .Answer(3)
                .Answer<OutcomePage>(3);
           
            outcomePage.VerifyOutcome("Contact your GP now");
            outcomePage.VerifyWorseningReveal(WorseningMessages.PrimaryCare);
            outcomePage.VerifyCareAdvice(new string[] { "Vomiting blood" });

            // Vomiting Blood should show in care advice but Vomiting on its own shouldn't
            Assert.IsFalse(Driver.ElementExists(By.Id("Advice_CX221222-Adult-Male")));

            outcomePage.CompareAndVerify("1");
        }
    }
}
