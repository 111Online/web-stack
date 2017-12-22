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
                .AnswerSuccessiveByOrder(1, 2)
                .AnswerAndVerifyQuestion(5, "Do you feel the worst you've ever felt in your life and have a new rash under your skin?")
                .AnswerAndVerifyQuestion(3, "Could you be pregnant?")
                .AnswerAndVerifyQuestion(3, "Have you brought up either of the following?")
                .AnswerAndVerifyQuestion(4, "Does your poo look black and tarry or red or maroon in colour?")
                .AnswerAndVerifyQuestion(4, "Do you have to stay completely still because of the pain?")
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }

        [Test]
        public void AgeTriageLogic_Over55()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioSex.Female, 56);

            questionPage.VerifyQuestion("Do you have any pain in your tummy (abdomen) as well as bleeding from your bottom (rectal bleeding)?");
            var outcomePage = questionPage.AnswerAndVerifyQuestion(1, "How bad is your pain?")
                .AnswerAndVerifyQuestion(1, "Have you got a sudden, intense ripping or tearing pain in your chest, tummy or back?")
                .AnswerAndVerifyQuestion(3, "Has a doctor diagnosed you with an aortic aneurysm or Marfan's syndrome?")
                .AnswerAndVerifyQuestion(4, "Have you got any of the symptoms of a heart attack?")
                .AnswerAndVerifyQuestion(5, "Do you feel the worst you've ever felt in your life and have a new rash under your skin?")
                .AnswerAndVerifyQuestion(3, "Have you brought up either of the following?")
                .AnswerAndVerifyQuestion(4, "Does your poo look black and tarry or red or maroon in colour?")
                .AnswerAndVerifyQuestion(4, "Do you have to stay completely still because of the pain?")
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }

        [Test]
        public void AgeTriageLogic_Under11()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioSex.Female, 10);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(1, 2)
                .AnswerAndVerifyQuestion(5, "Do you feel the worst you've ever felt in your life and have a new rash under your skin?")
                .AnswerAndVerifyQuestion(3, "Have you brought up either of the following?")
                .AnswerAndVerifyQuestion(4, "Does your poo look black and tarry or red or maroon in colour?")
                .AnswerAndVerifyQuestion(4, "Do you have to stay completely still because of the pain?")
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }
    }
}
