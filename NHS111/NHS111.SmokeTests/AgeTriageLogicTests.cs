﻿using NHS111.SmokeTest.Utils;
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
                .AnswerAndVerifyNextQuestion(1, "Do you have any of the symptoms of a heart attack?")
                .AnswerAndVerifyNextQuestion(5, "Are you so ill you've stopped doing your usual daily activities?")
                .AnswerAndVerifyNextQuestion(3, "Is there a chance you're pregnant?")
                .AnswerAndVerifyNextQuestion(3, "Have you vomited up either of the following?")
                .AnswerAndVerifyNextQuestion(4, "What does your poo look like?")
                .AnswerAndVerifyNextQuestion(4, "Is the pain so unbearable you can't move at all?")
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Phone 999 now for an ambulance");
        }

        [Test]
        public void AgeTriageLogic_Over55()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioSex.Female, 56);

            questionPage.VerifyQuestion("Have you got pain in your tummy as well as the bleeding from your bottom?");
            var outcomePage = questionPage.AnswerAndVerifyNextQuestion(1, "How bad is the pain?")
                .AnswerAndVerifyNextQuestion(1, "Have you got a severe ripping or tearing pain in your chest or back that came on suddenly?")
                .AnswerAndVerifyNextQuestion(3, "Has a doctor told you that you have either of the following?")
                .AnswerAndVerifyNextQuestion(4, "Do you have any of the symptoms of a heart attack?")
                .AnswerAndVerifyNextQuestion(5, "Are you so ill you've stopped doing your usual daily activities?")
                .AnswerAndVerifyNextQuestion(3, "Have you vomited up either of the following?")
                .AnswerAndVerifyNextQuestion(4, "What does your poo look like?")
                .AnswerAndVerifyNextQuestion(4, "Is the pain so unbearable you can't move at all?")
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Phone 999 now for an ambulance");
        }

        [Test]
        public void AgeTriageLogic_Under11()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Rectal Bleeding", TestScenerioSex.Female, 10);

            var outcomePage = questionPage
                .AnswerYes()
                .AnswerAndVerifyNextQuestion(1, "Do you have any of the symptoms of a heart attack?")
                .AnswerAndVerifyNextQuestion(5, "Are you so ill you've stopped doing your usual daily activities?")
                .AnswerAndVerifyNextQuestion(3, "Have you vomited up either of the following?")
                .AnswerAndVerifyNextQuestion(4, "What does your poo look like?")
                .AnswerAndVerifyNextQuestion(4, "Is the pain so unbearable you can't move at all?")
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("Phone 999 now for an ambulance");
        }
    }
}
