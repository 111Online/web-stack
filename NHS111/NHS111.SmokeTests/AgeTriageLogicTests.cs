using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests
{
    public class AgeTriageLogicTests : BaseTests
    {
        //TODO: Discuss actual question triggered by set / read nodes for age. Remove asserts for other questions, not impacted by read nodes.
        [Test]
        public void AgeTriageLogic_Over10()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioSex.Female, 11);

            var outcomePage = questionPage
                .AnswerYes()
                .AnswerAndVerifyQuestion(1, "Do you have any of the symptoms of a heart attack?")
                .AnswerAndVerifyQuestion(5, "Do you have a new rash that won't go away when you press a glass on it, and you also feel severely ill?")
                .AnswerAndVerifyQuestion(3, "Could you be pregnant?")
                .AnswerAndVerifyQuestion(3, "Have you vomited up either of the following?")
                .AnswerAndVerifyQuestion(4, "What does your poo look like?")
                .AnswerAndVerifyQuestion(4, "Is the pain so bad you need to keep completely still?")
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }

        [Test]
        public void AgeTriageLogic_Over55()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioSex.Female, 56);

            questionPage.VerifyQuestion("Have you got pain in your tummy as well as bleeding from your bottom?");
            var outcomePage = questionPage.AnswerAndVerifyQuestion(1, "How bad is the pain?")
                .AnswerAndVerifyQuestion(1, "Have you got a severe ripping or tearing pain in your chest or back that came on suddenly?")
                .AnswerAndVerifyQuestion(3, "Has a doctor told you that you have either of the following?")
                .AnswerAndVerifyQuestion(4, "Do you have any of the symptoms of a heart attack?")
                .AnswerAndVerifyQuestion(5, "Do you have a new rash that won't go away when you press a glass on it, and you also feel severely ill?")
                .AnswerAndVerifyQuestion(3, "Have you vomited up either of the following?")
                .AnswerAndVerifyQuestion(4, "What does your poo look like?")
                .AnswerAndVerifyQuestion(4, "Is the pain so bad you need to keep completely still?")
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }

        [Test]
        public void AgeTriageLogic_Under11()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioSex.Female, 10);

            var outcomePage = questionPage
                .AnswerYes()
                .AnswerAndVerifyQuestion(1, "Do you have any of the symptoms of a heart attack?")
                .AnswerAndVerifyQuestion(5, "Do you have a new rash that won't go away when you press a glass on it, and you also feel severely ill?")
                .AnswerAndVerifyQuestion(3, "Have you vomited up either of the following?")
                .AnswerAndVerifyQuestion(4, "What does your poo look like?")
                .AnswerAndVerifyQuestion(4, "Is the pain so bad you need to keep completely still?")
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }
    }
}
