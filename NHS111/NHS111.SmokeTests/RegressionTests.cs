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
        public void SplitQuestionJourneyThroughEachRoute()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            questionPage.VerifyQuestion("Have you hurt your head in the last 7 days?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(3)
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Phone 999 now for an ambulance");

            TestScenerios.LaunchTriageScenerio(Driver, "Headache", "Female", 49);

            //to be discussed with Andria
            //questionPage.VerifyQuestion("Could you be pregnant?");
            //outcomePage = questionPage.AnswerSuccessiveByOrder(3, 4)
            //    .Answer(1)
            //    .Answer(3)
            //    .Answer(5)
            //    .Answer(3)
            //    .Answer(4)
            //    .Answer(2)
            //    .Answer(3)
            //    .Answer(3)
            //    .Answer(3)
            //    .Answer(4)
            //    .Answer(1)
            //    .Answer(3)
            //    .Answer(4)
            //    .Answer(3)
            //    .Answer(1)
            //    .AnswerForDispostion<OutcomePage>("Within the next 6 hours");

            
            //outcomePage.VerifyOutcome("Speak to your GP practice today");

            TestScenerios.LaunchTriageScenerio(Driver, "Headache", "Female", 50);

            questionPage.VerifyQuestion("Is there a chance you're pregnant?");
            outcomePage = questionPage.AnswerSuccessiveByOrder(3, 5)
                .Answer(5)
                .Answer(3)
                .Answer(3)
                .Answer(2)
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerForDispostion<OutcomePage>("Yes");


            outcomePage.VerifyOutcome("Speak to your GP practice urgently");
        }

        [Test]
        public void QuestionDisplaysRationale()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Is there a chance you're pregnant?");
            questionPage.VerifyRationale();
        }
    }
}

