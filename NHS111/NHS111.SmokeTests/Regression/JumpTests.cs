using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests.Regression
{
    public class JumpTests : BaseTests
    {
        [Test]
        public void Pt8JumpToDx35()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Tiredness (Fatigue)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Have you got a raised temperature now or have you had one at any time since the tiredness started?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerSuccessiveByOrder(4, 2)
                .Answer(3)
                .Answer(4)
                .Answer(3)
                .AnswerSuccessiveByOrder(5, 2)
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerForDispostion<OutcomePage>("Alcohol");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx 140148 Tx221449 and Tx222008
        public void Pt8ViaBehaviourChangeTx140148Tx221449Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(11)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx140148 and Tx222023 No Dx String
        public void Pt8ViaBehaviourChangeTx140148Tx222023NoDx()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(2)
                .AnswerSuccessiveNo(7)
                .Answer(2)
                .AnswerSuccessiveNo(8)
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx221449 and Tx222006
        public void Pt8ViaBehaviourChangeTx221449Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(12)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx221449 and Tx222007
        public void Pt8ViaBehaviourChangeTx221449Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(12)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx221449 and Tx222008
        public void Pt8ViaBehaviourChangeTx221449Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx221449 and Tx222009
        public void Pt8ViaBehaviourChangeTx221449Tx222009()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(3)
                .AnswerForDispostion<OutcomePage>("I've stopped taking a medicine");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222023 and Tx222006
        public void Pt8ViaBehaviourChangeTx222023Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(5)
                .Answer(1)
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222023 and Tx222007
        public void Pt8ViaBehaviourChangeTx222023Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(5)
                .Answer(1)
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222023 and Tx222008
        public void Pt8ViaBehaviourChangeTx222023Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(5)
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222024 and Tx222006
        public void Pt8ViaBehaviourChangeTx222024Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(5)
                .Answer(2)
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222024 and Tx222007
        public void Pt8ViaBehaviourChangeTx222024Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(5)
                .Answer(2)
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222024 and Tx222008
        public void Pt8ViaBehaviourChangeTx222024Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(5)
                .Answer(2)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222025 and Tx222006
        public void Pt8ViaBehaviourChangeTx222025Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(5)
                .Answer(3)
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222025 and Tx222007
        public void Pt8ViaBehaviourChangeTx222025Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(5)
                .Answer(3)
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222025 and Tx222008
        public void Pt8ViaBehaviourChangeTx222025Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(5)
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222026 and Tx222006
        public void Pt8ViaBehaviourChangeTx222026Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(6)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222026 and Tx222007
        public void Pt8ViaBehaviourChangeTx222026Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(6)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222026 and Tx222008
        public void Pt8ViaBehaviourChangeTx222026Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(6)
                .AnswerYes()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222027 and Tx222006
        public void Pt8ViaBehaviourChangeTx222027Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(7)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222027 and Tx222007
        public void Pt8ViaBehaviourChangeTx222027Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(7)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222027 and Tx222008
        public void Pt8ViaBehaviourChangeTx222027Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(7)
                .AnswerYes()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222028 and Tx222006
        public void Pt8ViaBehaviourChangeTx222028Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(8)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222027 and Tx222007
        public void Pt8ViaBehaviourChangeTx222028Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(8)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222027 and Tx222008
        public void Pt8ViaBehaviourChangeTx222028Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(8)
                .AnswerYes()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222031 and Tx222006
        public void Pt8ViaBehaviourChangeTx222031Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(9)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222031 and Tx222007
        public void Pt8ViaBehaviourChangeTx222031Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(9)
                .AnswerYes()
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222031 and Tx222008
        public void Pt8ViaBehaviourChangeTx222031Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(9)
                .AnswerYes()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222048 and Tx222006
        public void Pt8ViaBehaviourChangeTx222048Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(10)
                .Answer(2)
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222048 and Tx222007
        public void Pt8ViaBehaviourChangeTx222048Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(10)
                .Answer(2)
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222048 and Tx222008
        public void Pt8ViaBehaviourChangeTx222048Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(10)
                .Answer(2)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222049 and Tx222006
        public void Pt8ViaBehaviourChangeTx222049Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(10)
                .Answer(1)
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222049 and Tx222007
        public void Pt8ViaBehaviourChangeTx222049Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(10)
                .Answer(1)
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx222049 and Tx222008
        public void Pt8ViaBehaviourChangeTx222049Tx222008()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Behaviour Change", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(10)
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222027 and Tx222006 FA
        public void Pt8ViaHeadacheTx222027Tx222006Fa()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerYes()
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .Answer(3)
                .Answer(5)
                .Answer(4)
                .AnswerNo()
                .Answer(3)
                .Answer(7)
                .AnswerNo()
                .Answer(2)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222027 and Tx222006 MC
        public void Pt8ViaHeadacheTx222027Tx222006Mc()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Child);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerSuccessiveNo(2)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .Answer(3)
                .Answer(5)
                .AnswerSuccessiveNo(2)
                .Answer(3)
                .Answer(7)
                .AnswerNo()
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222027 and Tx222006
        public void Pt8ViaHeadacheTx222027Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .Answer(3)
                .Answer(5)
                .Answer(4)
                .AnswerNo()
                .Answer(3)
                .Answer(7)
                .AnswerNo()
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222027 and Tx222007 FA
        public void Pt8ViaHeadacheTx222027Tx222007Fa()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerYes()
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .Answer(3)
                .Answer(5)
                .Answer(4)
                .AnswerNo()
                .Answer(3)
                .Answer(7)
                .AnswerNo()
                .Answer(2)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222027 and Tx222007 MC
        public void Pt8ViaHeadacheTx222027Tx222007Mc()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Child);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerSuccessiveNo(2)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .Answer(3)
                .Answer(5)
                .AnswerSuccessiveNo(2)
                .Answer(3)
                .Answer(7)
                .AnswerNo()
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222027 and Tx222007
        public void Pt8ViaHeadacheTx222027Tx222007()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .Answer(3)
                .Answer(5)
                .Answer(4)
                .AnswerNo()
                .Answer(3)
                .Answer(7)
                .AnswerNo()
                .Answer(1)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222031 and Tx222006 FA
        public void Pt8ViaHeadacheTx222031Tx222006Fa()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerYes()
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .Answer(3)
                .Answer(5)
                .Answer(4)
                .AnswerNo()
                .Answer(3)
                .Answer(7)
                .AnswerNo()
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222031 and Tx222006 MC
        public void Pt8ViaHeadacheTx222031Tx222006Mc()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Child);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerSuccessiveNo(2)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .Answer(3)
                .Answer(5)
                .AnswerSuccessiveNo(2)
                .Answer(3)
                .Answer(7)
                .AnswerNo()
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222031 and Tx222006
        public void Pt8ViaHeadacheTx222031Tx222006()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .AnswerSuccessiveNo(4)
                .Answer(3)
                .Answer(5)
                .Answer(4)
                .AnswerNo()
                .Answer(3)
                .Answer(7)
                .AnswerNo()
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Tremor via Age variable and 2 strings
        public void Pt8ViaTremorViaAgeVariable2Strings()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Tremor", TestScenerioSex.Male, 13);

            var outcomePage = questionPage
                .AnswerNo()
                .Answer(3)
                .AnswerSuccessiveNo(3)
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyDispositionCode("Dx06");

            var genderPage = outcomePage.NavigateBackToGenderPage();

            genderPage.VerifyHeader();
            genderPage.SelectSexAndAge(TestScenerioSex.Male, 14);

            var searchpage = genderPage.NextPage();
            var newQuestionPage = searchpage.TypeSearchTextAndSelect("Tremor");

            var newOutcomePage = newQuestionPage
                .AnswerNo()
                .Answer(3)
                .AnswerSuccessiveNo(3)
                .AnswerForDispostion<OutcomePage>("Yes");

            newOutcomePage.VerifyDispositionCode("Dx35");
        }
    }
}
