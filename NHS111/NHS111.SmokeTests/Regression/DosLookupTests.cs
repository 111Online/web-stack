using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests.Regression
{
    public class DosLookupTests : BaseTests
    {
        [Test]
        //PT8 via Behaviour Change Tx222027 and Tx222006
        public void Pt8ViaBehaviourChangeTx222027Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(7)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222027 and Tx222007
        public void Pt8ViaBehaviourChangeTx222027Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(7)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222027 and Tx222008
        public void Pt8ViaBehaviourChangeTx222027Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(7)
                .AnswerYes()
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222028 and Tx222006
        public void Pt8ViaBehaviourChangeTx222028Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(8)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222028 and Tx222007
        public void Pt8ViaBehaviourChangeTx222028Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(8)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222028 and Tx222008
        public void Pt8ViaBehaviourChangeTx222028Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(8)
                .AnswerYes()
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222031 and Tx222006
        public void Pt8ViaBehaviourChangeTx222031Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(9)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222031 and Tx222007
        public void Pt8ViaBehaviourChangeTx222031Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(9)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222031 and Tx222008
        public void Pt8ViaBehaviourChangeTx222031Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(9)
                .AnswerYes()
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }
    }
}
