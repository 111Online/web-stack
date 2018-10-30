namespace NHS111.SmokeTests
{
    using System.Linq;
    using SmokeTest.Utils;
    using NUnit.Framework;
    using OpenQA.Selenium;

    [TestFixture]
    public class EndpointJourneyTests
        : BaseTests {

        [TestCase("Male", 22, "Headache", new[] {3, 3, 3, 5, 3, 3, 3, 1}, "Dx02", TestName = "Can reach Dx02")]
        [TestCase("Male", 24, "Sexual Concerns", new[] {3, 4, 3, 3, 3, 4, 4, 1, 1, 3}, "Dx03", TestName = "Can reach Dx03")]
        [TestCase("Male", 6, "Object, Ingested or Inhaled", new[] {1, 3, 3, 5, 3, 5, 3, 3, 3, 3, 3, 3, 3}, "Dx89", TestName = "Can reach Dx89")]
        [TestCase("Female", 16, "Mental Health Problems", new[] {1, 5, 3, 5, 3, 1, 4}, "Dx92", TestName = "Can reach Dx92")]
        [TestCase("Female", 22, "Sexual or Menstrual Concerns", new[] {1}, "Dx94", TestName = "Can reach Dx94")]
        public void TestOutcomes(string sex, int age, string pathwayTitle, int[] answers, string expectedDxCode) {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, pathwayTitle, sex, age);

            for (var i = 0; i < answers.Length - 1; i++) {
                questionPage.Answer(answers[i]);
            }

            var outcomePage = questionPage.AnswerForDispostion<OutcomePage>(answers.Last());
            //take screenshot
            outcomePage.VerifyDispositionCode(expectedDxCode);
        }

        [Test]
        public void Call999EndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Skin Problems", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("What is the main problem?");
            var outcomePage =  questionPage
                .Answer(1)
                .AnswerSuccessiveByOrder(1,2)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Phone 999 now for an ambulance");
        }

        [Test]
        public void Call999_WithoutITKResult_DisplaysDispositionPage()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            
                var outcomePage = questionPage
                                                   .Answer(1)
                                .Answer(1)
                                .Answer(3)
                                .Answer(1)
                                .AnswerForDeadEnd<OutcomePage>("Yes");
            
             outcomePage.VerifyOutcome("Phone 999 now for an ambulance");
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
                .AnswerForDispostion<OutcomePage>("Yes");
 
            outcomePage.VerifyOutcome("Your answers suggest you should contact a pharmacist within 12 hours");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyFindService(FindServiceTypes.Pharmacy);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new[] { "Eye discharge" });
        }

        [Test]
        public void HomeCareEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Cold or Flu (Declared)", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Are you so ill you've stopped doing your usual daily activities?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(3) //mers
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .Answer(4)
                .Answer(6)
                .Answer(3)
                .AnswerForDispostion<OutcomePage>(3);

            outcomePage.VerifyOutcome("Based on your answers, you can look after yourself and don't need to see a healthcare professional");
           // outcomePage.VerifyHeaderOtherInfo("Based on your answers you do not need to see a healthcare profesional at this time.\r\nPlease see the advice below on how to look after yourself");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111);
            
        }

        [Test]
        public void DentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Do you have dental bleeding, toothache or a different dental problem?");
            var postcodeFirstPage = questionPage
                .Answer(2)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 5)
                .AnswerForDispostion<PostcodeFirstPage>("No - I've not taken any painkillers");

            //postcodeFirstPage.EnterPostCodeAndSubmit("LS17 7NZ");

            postcodeFirstPage.VerifyOutcome("See your dentist in the next few days");
            postcodeFirstPage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            postcodeFirstPage.VerifyCareAdviceHeader("What you can do in the meantime");
            postcodeFirstPage.VerifyCareAdvice(new string[] { "Toothache", "Medication, pain and/or fever" });
        }

        [Test]
        public void EmergencyDentalEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Do you have dental bleeding, toothache or a different dental problem?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .Answer(1)
                .Answer(4)
                .AnswerForDispostion<OutcomePage>("It's getting worse");

            outcomePage.VerifyOutcome("Your answers suggest you need urgent attention for your dental problem within 4 hours");
            outcomePage.VerifyFindService(FindServiceTypes.EmergencyDental);
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Tooth extraction" });
        }

        [Test]
        public void AccidentAndEmergencyEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Have you hurt your head in the last 7 days?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3,3)
                .Answer(5)
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Go to an emergency treatment centre urgently");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Headache", "Breathlessness", "Medication, pain and/or fever" });
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
                .AnswerSuccessiveByOrder(3,2)
                .Answer(4)
                .AnswerSuccessiveByOrder(3,3)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("Your answers suggest you should see an optician within 3 days");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyFindService(FindServiceTypes.Optician);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] {"Eye discharge", "Medication, pain and/or fever" });
        }

        [Test]
        public void GPEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Diarrhoea and Vomiting", TestScenerioSex.Male, TestScenerioAgeGroups.Child);

            questionPage.VerifyQuestion("Do any of these apply to your sick (vomit)?");
            var outcomePage = questionPage
                .Answer(5)
                .AnswerSuccessiveByOrder(3,2)
                .Answer(4)
                .AnswerSuccessiveByOrder(3,5)
                .AnswerSuccessiveByOrder(4,2)
                .AnswerSuccessiveByOrder(3,3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 2)
                .Answer(5)
                .AnswerForDispostion<OutcomePage>("Yes - 1 week or more");

            outcomePage.VerifyOutcome("Your answers suggest that you should talk to your own GP in 3 working days if you are not feeling better");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Diarrhoea & Vomiting" });
        }

        [Test]
        public void JumpToRemappedMentalHealthPathway()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Tiredness (Fatigue)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Have you got a raised temperature now or have you had one at any time since the tiredness started?");
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
               .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome(OutcomePage.BookCallBackText);
        }

        [Test]
        public void GPOOHEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Have you hurt your head in the last 7 days?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 4)
                //.AnswerSuccessiveByOrder(1,2)
                .AnswerForDispostion<PostcodeFirstPage>("Yes");
           
            //outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyOutcome("Speak to your GP practice urgently");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Medication, pain and/or fever", "Headache" });
       }

        [Test]
        public void MidwifeEndpointJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Is there a chance you're pregnant?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .Answer(3)
                .Answer(3)
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyFindService(FindServiceTypes.Midwife);
            outcomePage.VerifyOutcome("Your answers suggest you should speak to your midwife within 1 hour");
            outcomePage.VerifyWorseningPanel(WorseningMessages.Call111PostCodeFirst);
            outcomePage.VerifyCareAdviceHeader("What you can do in the meantime");
            outcomePage.VerifyCareAdvice(new string[] { "Medication, pain and/or fever", "Headache" });
        }

        [Test]
        public void DeadEndJourney()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Mental Health Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Do you have a diagnosed mental health condition that's got worse?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(5)
                .Answer(3)
                .Answer(5)
                .AnswerForDeadEnd<DeadEndPage>("Yes");

            outcomePage.VerifyOutcome("Call 111 to speak to an adviser now");
        }
    }
}
