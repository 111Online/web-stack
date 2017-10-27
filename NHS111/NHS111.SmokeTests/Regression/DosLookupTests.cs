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

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyLayoutPagePresent();
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

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyLayoutPagePresent();
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

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyLayoutPagePresent();
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

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via Behaviour Change Tx222028 and Tx222007
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

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via Behaviour Change Tx222028 and Tx222008
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

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyLayoutPagePresent();
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

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyLayoutPagePresent();
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

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyLayoutPagePresent();
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

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via Headache Tx222031 and Tx222007 FA
        public void Pt8ViaHeadacheTx222031Tx222007Fa()
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
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via Headache Tx222031 and Tx222007 MC
        public void Pt8ViaHeadacheTx222031Tx222007Mc()
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
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via Headache Tx222031 and Tx222007
        public void Pt8ViaHeadacheTx222031Tx222007()
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
                .AnswerForDispostion<OutcomePage>("Yes");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via Headache via Digital split question Tx1054
        public void Pt8ViaHeadacheViaDigitalSplitQuestionTx1054()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(2)
                .AnswerYes()
                .Answer(3)
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

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via LowMood_Depression via operator branch
        public void Pt8ViaLowMoodDepressionViaOperatorBranch()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Mental Health Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(2)
                .Answer(1)
                .AnswerSuccessiveNo(12)
                .AnswerForDispostion<OutcomePage>("Less than 2 weeks");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via MHP jump to PW1738 Drug_Solvent_Alcohol_Misuse then 3 strings
        public void Pt8ViaMhpJumpPw1738DrugSolventAlcoholMisuse3Strings()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Mental Health Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(2)
                .Answer(4)
                .Answer(2)
                .AnswerSuccessiveNo(2)
                .AnswerForDispostion<OutcomePage>("Specialist help");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via MHP jump to PW1738.700 Drug_Solvent_Alcohol_Misuse then 3 strings
        public void Pt8ViaMhpJumpPw1738_700DrugSolventAlcoholMisuse3Strings()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Mental Health Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(2)
                .Answer(4)
                .Answer(1)
                .AnswerNo()
                .AnswerForDispostion<OutcomePage>("Information only");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via MHP PW1752 Anxiety_Stress_Panic then 2 strings
        public void Pt8ViaMhpJumpPw1752AnxietyStressPanic2Strings()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Mental Health Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(2)
                .Answer(4)
                .AnswerNo()
                .Answer(1)
                .AnswerSuccessiveNo(3)
                .AnswerForDispostion<OutcomePage>("I've stopped taking a medicine");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via MHP PW752 headache jump to PW755 Headache then via Digital split question Tx1054
        public void Pt8ViaMhpPw752HeadacheJumpPw755HeadacheViaDigitalSplitQuestionTx1054()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(4)
                .AnswerYes()
                .Answer(3)
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

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via search no pathway jumps PW1072 Tiredness then 2 strings
        public void Pt8ViaSearchNoPathwayJumpsPw1072Tiredness2Strings()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Tiredness (Fatigue)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(3)
                .Answer(4)
                .AnswerSuccessiveNo(10)
                .AnswerForDispostion<OutcomePage>("Drugs");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }

        [Test]
        //PT8 via search no pathway jumps PW1072 Tiredness then 2 strings
        public void Pt8ViaSleepDifficultiesJumpPw1750KnownMentalHealthProblemsWorsening3Strings()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Sleep Difficulties", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerYes()
                .AnswerSuccessiveNo(5)
                .Answer(3)
                .AnswerNo()
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("No");

            outcomePage.VerifyOutcome("A nurse from 111 will phone you");
            outcomePage.VerifyDispositionCode("Dx35");

            outcomePage.EnterPostCodeAndSubmit("ls17 7nz");

            outcomePage.VerifyLayoutPagePresent();
        }
    }
}
