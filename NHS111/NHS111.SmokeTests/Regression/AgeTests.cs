using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests.Regression
{
    public class AgeTests : BaseTests
    {
        [Test]
        //age logic over 10 staging
        public void AgeLogicOver10()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioGender.Female, 11);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(1, 2)
                .AnswerAndValidateQuestion(5, "Do you feel the worst you've ever felt in your life and have a new rash under your skin?")
                .AnswerAndValidateQuestion(3, "Is there a chance you are pregnant?")
                .AnswerAndValidateQuestion(3, "Have you had any blood in your sick (vomit)?")
                .AnswerAndValidateQuestion(4, "Does your poo look black and tarry or red or maroon in colour?")
                .AnswerAndValidateQuestion(4, "Do you have to stay completely still because of the pain?")
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }

        [Test]
        //age logic over 55 staging
        public void AgeLogicOver55()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioGender.Female, 56);

            var outcomePage = questionPage
                .ValidateQuestion("Do you have any pain in your tummy (abdomen) as well as bleeding from your bottom (rectal bleeding)?")
                .AnswerAndValidateQuestion(1, "How bad is your pain?")
                .AnswerAndValidateQuestion(1, "Have you got a sudden, intense ripping or tearing pain in your chest, tummy or back?")
                .AnswerAndValidateQuestion(3, "Has a doctor diagnosed you with an aortic aneurysm or Marfan's syndrome?")
                .AnswerAndValidateQuestion(4, "Have you got any of the symptoms of a heart attack?")
                .AnswerAndValidateQuestion(5, "Do you feel the worst you've ever felt in your life and have a new rash under your skin?")
                .AnswerAndValidateQuestion(3, "Have you had any blood in your sick (vomit)?")
                .AnswerAndValidateQuestion(4, "Does your poo look black and tarry or red or maroon in colour?")
                .AnswerAndValidateQuestion(4, "Do you have to stay completely still because of the pain?")
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }

        [Test]
        //age logic under 11 staging
        public void AgeLogicUnder11()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioGender.Female, 10);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(1, 2)
                .AnswerAndValidateQuestion(5, "Do you feel the worst you've ever felt in your life and have a new rash under your skin?")
                .AnswerAndValidateQuestion(3, "Have you had any blood in your sick (vomit)?")
                .AnswerAndValidateQuestion(4, "Does your poo look black and tarry or red or maroon in colour?")
                .AnswerAndValidateQuestion(4, "Do you have to stay completely still because of the pain?")
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");
        }
    }
}
