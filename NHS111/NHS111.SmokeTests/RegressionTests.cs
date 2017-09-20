using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class RegressionTests : BaseTests
    {
        [Test]
        public void PT8JumpToDx35()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Tiredness (Fatigue)",
                TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion(
                "Have you got a raised temperature now or have you had one at any time since the tiredness started?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 4)
                .Answer(4)
                .Answer(4)
                .Answer(3)
                .Answer(4)
                .Answer(3)
                .AnswerSuccessiveByOrder(5, 2)
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerForDispostion("Alcohol");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
        }

        [Test]
        public void PathwayNotFound()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver,
                "Wound Problems, Plaster Casts, Tubes and Metal Appliances", TestScenerioGender.Male,
                TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Is the problem to do with any of these?");
            var outcomePage = questionPage

                .AnswerForDispostion("A tube or drain");

            outcomePage.VerifyPathwayNotFound();
        }


        [Test]
        public void SplitQuestionNavigateBackDisplaysCorrectCareAdvice()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", "Female", 49);

            var outcomePage = questionPage.ValidateQuestion("Is there a chance you are pregnant?")

                .AnswerSuccessiveByOrder(3, 4)
                .Answer(1)
                .Answer(3)
                .Answer(5)
                .Answer(3)
                .Answer(4)
                .Answer(2)
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .Answer(4)
                .Answer(1)
                .Answer(3)
                .Answer(4)
                .AnswerForDispostion(1);

            var newOutcome = outcomePage.NavigateBack()
                .Answer(3, false)
                .Answer(1)


                .AnswerForDispostion("Within the next 6 hours");

            newOutcome.EnterPostCodeAndSubmit("LS17 7NZ");

            newOutcome.VerifyOutcome("Speak to your GP practice today");
            newOutcome.VerifyCareAdvice(new[] {"Medication, next dose", "Medication, pain and/or fever", "Headache"});


        }

        [Test]
        public void SplitQuestionJourneyThroughEachRoute()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioGender.Male,
                TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Have you hurt or banged your head in the last 7 days?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(3)
                .Answer(1)
                .AnswerForDispostion("Yes - I have a rash that doesn't disappear if I press it");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");

            TestScenerios.LaunchTriageScenerio(Driver, "Headache", "Female", 49);

            questionPage.ValidateQuestion("Is there a chance you are pregnant?")

                .AnswerSuccessiveByOrder(3, 4)
                .Answer(1)
                .Answer(3)
                .Answer(5)
                .Answer(3)
                .Answer(4)
                .Answer(2)
                .Answer(3)
                .Answer(3)
                .Answer(3)
                .Answer(4)
                .Answer(1)
                .Answer(3)
                .Answer(4)
                //.Answer(1)
                //.NavigateBack()
                .Answer(3)
                .Answer(1)


                .AnswerForDispostion("Within the next 6 hours");

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyOutcome("Speak to your GP practice today");


            TestScenerios.LaunchTriageScenerio(Driver, "Headache", "Female", 50);

            questionPage.ValidateQuestion("Is there a chance you are pregnant?")

                .AnswerSuccessiveByOrder(3, 5)
                .Answer(5)
                .Answer(3)
                .Answer(4)
                .Answer(2)
                .AnswerSuccessiveByOrder(3, 3)

                .AnswerForDispostion("Yes");

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");


            outcomePage.VerifyOutcome("Speak to your GP practice urgently");
        }

    }
}

