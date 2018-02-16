using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class RegressionTests : BaseTests
    {
        [Test]
        public void PathwayNotFound()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver,
                "Wound Problems, Plaster Casts, Tubes and Metal Appliances", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Is the problem to do with any of these?");
            var outcomePage = questionPage

                .AnswerForDispostion<OutcomePage>("A tube or drain");

            outcomePage.VerifyPathwayNotFound();
        }


        [Test]
        public void SplitQuestionNavigateBackDisplaysCorrectCareAdvice()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", "Female", 49);

            questionPage.VerifyQuestion("Could you be pregnant?");
            var outcomePage = questionPage.AnswerSuccessiveByOrder(3, 4)
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
                .AnswerForDispostion<OutcomePage>(1);

            var newOutcome = outcomePage.NavigateBack()
                .Answer(3, false)
                .Answer(1)
                .AnswerForDispostion<PostcodeFirstPage>("Within the next 6 hours");

            newOutcome.EnterPostCodeAndSubmit("LS17 7NZ");

            newOutcome.VerifyOutcome("Speak to your GP practice today");
            newOutcome.VerifyCareAdvice(new[] {"Medication, next dose", "Medication, pain and/or fever", "Headache"});
        }

        [Test]
        public void SplitQuestionJourneyThroughEachRoute()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            questionPage.VerifyQuestion("Have you hurt your head in the last 7 days?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(3)
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");

            TestScenerios.LaunchTriageScenerio(Driver, "Headache", "Female", 49);

            questionPage.VerifyQuestion("Could you be pregnant?");
            var postcodeFirstPage = questionPage.AnswerSuccessiveByOrder(3, 4)
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
                .Answer(3)
                .Answer(1)
                .AnswerForDispostion<PostcodeFirstPage>("Within the next 6 hours");

            postcodeFirstPage.EnterPostCodeAndSubmit("LS17 7NZ");
            
            postcodeFirstPage.VerifyOutcome("Speak to your GP practice today");

            TestScenerios.LaunchTriageScenerio(Driver, "Headache", "Female", 50);

            questionPage.VerifyQuestion("Could you be pregnant?");
            postcodeFirstPage = questionPage.AnswerSuccessiveByOrder(3, 5)
                .Answer(5)
                .Answer(3)
                .Answer(4)
                .Answer(2)
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerForDispostion<PostcodeFirstPage>("Yes");

            postcodeFirstPage.EnterPostCodeAndSubmit("LS17 7NZ");

            postcodeFirstPage.VerifyOutcome("Speak to your GP practice urgently");
        }

        [Test]
        public void QuestionDisplaysRationale()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Could you be pregnant?");
            questionPage.VerifyRationale();
        }
    }
}

