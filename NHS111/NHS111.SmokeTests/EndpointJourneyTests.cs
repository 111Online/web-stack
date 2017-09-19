using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class EndpointJourneyTests : BaseTests
    {
        [Test]
        public void Call999EndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Skin Problems", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("What is the main problem?");
            var outcomePage =  questionPage
                .Answer(1)
                .AnswerSuccessiveByOrder(1,2)
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }

        [Test]
        public void PharmacyEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Eye or Eyelid Problems", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .AnswerSuccessiveByOrder(1, 1)
                .AnswerSuccessiveByOrder(3, 6)
                .AnswerForDispostion("Yes");
 
            outcomePage.VerifyOutcome("Your answers suggest you should contact a pharmacist within 12 hours");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyFindService(FindServiceTypes.Pharmacy);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new[] { "Eye discharge" });
        }

        [Test]
        public void HomeCareEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Cold or Flu (Declared)", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Do you feel so ill that you can't think of anything else?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(4)
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .Answer(4)
                .Answer(6)
                .Answer(3)
                .AnswerForDispostion(3);

            outcomePage.VerifyOutcome("Based on your answers, you can look after yourself and don't need to see a healthcare professional");
           // outcomePage.VerifyHeaderOtherInfo("Based on your answers you do not need to see a healthcare profesional at this time.\r\nPlease see the advice below on how to look after yourself");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            
        }

        [Test]
        public void DentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Do you have dental bleeding, toothache or a different dental problem?");
            var outcomePage = questionPage
                .Answer(2)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 5)
                .AnswerForDispostion("No - I've not taken any painkillers");

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyOutcome("See a dentist in the next few days");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Toothache", "Medication, pain and/or fever" });
        }

        [Test]
        public void EmergencyDentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Do you have dental bleeding, toothache or a different dental problem?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .Answer(1)
                .Answer(4)
                .AnswerForDispostion("It's getting worse");

            outcomePage.VerifyOutcome("Your answers suggest you need urgent attention for your dental problem within 4 hours");
            outcomePage.VerifyFindService(FindServiceTypes.EmergencyDental);
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Tooth extraction" });
        }

        [Test]
        public void AccidentAndEmergencyEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Have you hurt or banged your head in the last 7 days?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3,3)
                .Answer(5)
                .Answer(3)
                .Answer(4)
                .Answer(3)
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you need urgent medical attention within 1 hour");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call999);
            outcomePage.VerifyFindService(FindServiceTypes.AccidentAndEmergency);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Headache", "Breathlessness", "Medication, pain and/or fever" });
        }

        [Test]
        public void OpticianEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Eye or Eyelid Problems", TestScenerioGender.Female, TestScenerioAgeGroups.Child);

            questionPage.ValidateQuestion("What is the main problem?");
            var outcomePage = questionPage
                .Answer(5)
                .Answer(3)
                .Answer(1)
                .Answer(5)
                .Answer(3)
                .Answer(2)
                .Answer(4)
                .Answer(3)
                .Answer(4)
                .AnswerSuccessiveByOrder(3,2)
                .Answer(4)
                .AnswerSuccessiveByOrder(3,3)
                .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("Your answers suggest you should see an optician within 3 days");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyFindService(FindServiceTypes.Optician);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] {"Eye discharge", "Medication, pain and/or fever" });
        }

        [Test]
        public void GPEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Diarrhoea and Vomiting", TestScenerioGender.Male, TestScenerioAgeGroups.Child);

            questionPage.ValidateQuestion("Have you had any blood in your sick (vomit)?");
            var outcomePage = questionPage
                .Answer(4)
                .AnswerSuccessiveByOrder(3,2)
                .Answer(5)
                .Answer(4)
                .AnswerSuccessiveByOrder(3,4)
                .AnswerSuccessiveByOrder(4,2)
                .AnswerSuccessiveByOrder(3,3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(5)
                .AnswerForDispostion("Yes - 1 week or more");

            outcomePage.VerifyOutcome("Your answers suggest that you should talk to your own GP in 3 working days if you are not feeling better");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Diarrhoea & Vomiting" });
        }

        [Test]
        public void JumpToRemappedMentalHealthPathway()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Tiredness (Fatigue)", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Have you got a raised temperature now or have you had one at any time since the tiredness started?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3,4)
               .AnswerSuccessiveByOrder(4, 2)
               .Answer(2)
               .Answer(3)
               .Answer(2)
               .AnswerSuccessiveByOrder(5,2)
               .AnswerSuccessiveByOrder(2,4)
               .Answer(3)
               .Answer(1)
               .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
        }

        [Test]
        public void GPOOHEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Have you hurt or banged your head in the last 7 days?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .Answer(1)
                .Answer(3)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 2)
                .AnswerSuccessiveByOrder(1,2)
                .AnswerForDispostion("Yes");
           
            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyOutcome("You should speak to your GP practice within the next 2 hours");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Medication, pain and/or fever", "Headache" });
       }

        [Test]
        public void MidwifeEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioGender.Female, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Is there a chance you are pregnant?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .Answer(3)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerForDispostion("No");

            outcomePage.VerifyFindService(FindServiceTypes.Midwife);
            outcomePage.VerifyOutcome("Your answers suggest you should speak to your midwife within 1 hour");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Medication, pain and/or fever", "Headache" });
        }


    }
}
