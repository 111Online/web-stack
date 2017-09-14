using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests.Regression
{
    public class JumpTests : BaseTests
    {
        [Test]
        public void Pt8JumpToDx35()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Tiredness (Fatigue)", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Have you got a raised temperature now or have you had one at any time since the tiredness started?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerSuccessiveByOrder(4, 2)
                .Answer(3)
                .Answer(4)
                .Answer(3)
                .AnswerSuccessiveByOrder(5, 2)
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerForDispostion("Alcohol");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
        }

        [Test]
        //PT8 via Behaviour Change Tx 140148 Tx221449 and Tx222008
        public void Pt8ViaBehaviourChangeTx140148Tx221449Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(11)
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx140148 and Tx222023 No Dx String
        public void Pt8ViaBehaviourChangeTx140148Tx222023NoDx()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(2)
                .AnswerSuccessiveNo(7)
                .Answer(2)
                .AnswerSuccessiveNo(8)
                .Answer(1)
                .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx221449 and Tx222006
        public void Pt8ViaBehaviourChangeTx221449Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(12)
                .AnswerForDispostion("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx221449 and Tx222007
        public void Pt8ViaBehaviourChangeTx221449Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(12)
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx221449 and Tx222008
        public void Pt8ViaBehaviourChangeTx221449Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .AnswerForDispostion("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx221449 and Tx222009
        public void Pt8ViaBehaviourChangeTx221449Tx222009()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(3)
                .AnswerForDispostion("I've stopped taking a medicine");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }
    }
}
