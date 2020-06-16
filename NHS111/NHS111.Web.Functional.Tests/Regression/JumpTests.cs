using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests.Regression
{
    public class JumpTests : BaseTests
    {
        [Test]
        public void Pt8JumpToDx05()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Mental Health Problems", TestScenarioSex.Male, TestScenarioAgeGroups.Adult);

            questionPage.VerifyQuestion("Do you have a diagnosed mental health problem that\'s got worse?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(4)
                .Answer(1)
                .Answer(4)
                .Answer(3)
                .Answer<OutcomePage>("Yes");

            outcomePage.VerifyDispositionCode("Dx05");
        }

        [Test]
        public void Pt8JumpToDx35()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Tiredness (Fatigue)", TestScenarioSex.Male, TestScenarioAgeGroups.Adult);

            questionPage.VerifyQuestion("Do you have a new continuous cough?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 6)
                .AnswerSuccessiveByOrder(4, 2)
                .Answer(3)
                .Answer(4)
                .Answer(3)
                .AnswerSuccessiveByOrder(5, 2)
                .AnswerSuccessiveByOrder(3, 4)
                .Answer<OutcomePage>("Alcohol");

            outcomePage.VerifyOutcome(OutcomePage.BookCallBackText);
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx 140148 Tx221449 and Tx222008
        public void Pt8JumpViaBehaviourChangePathway()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Behaviour Change", TestScenarioSex.Male, TestScenarioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(6)
                .Answer(3)
                .AnswerSuccessiveNo(13)
                .Answer<OutcomePage>("Yes");

            outcomePage.VerifyOutcome(OutcomePage.BookCallBackText);
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Behaviour Change Tx140148 and Tx222023 No Dx String
        public void Pt8JumpViaBehaviourChangePathwayNoDx()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Behaviour Change", TestScenarioSex.Male, TestScenarioAgeGroups.Adult);

            var outcomePage = questionPage
                .Answer(3)
                .Answer(5)
                .Answer(3)
                .Answer(1)
                .Answer(3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 2)
                .AnswerSuccessiveByOrder(4, 2)
                .AnswerSuccessiveNo(6)
                .Answer(4)
                .Answer(5)
                .Answer(4)
                .Answer(3)
                .Answer(5)
                .Answer(1)
                .Answer<OutcomePage>("No");

            outcomePage.VerifyOutcome(OutcomePage.BookCallBackText);
            outcomePage.VerifyDispositionCode("Dx35");
        }


        [Test]
        //PT8 via Headache Tx222027 and Tx222006 FA
        public void Pt8JumpViaHeadachePathwayFemaleAdult()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Headache", TestScenarioSex.Female, TestScenarioAgeGroups.Adult);

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
                .Answer<OutcomePage>("No");

            outcomePage.VerifyOutcome(OutcomePage.BookCallBackText);
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Headache Tx222027 and Tx222006 MC
        public void Pt8JumpViaHeadachePathwayMaleChild()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Headache", TestScenarioSex.Male, TestScenarioAgeGroups.Child);

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
                .Answer<OutcomePage>("No");

            outcomePage.VerifyOutcome(OutcomePage.BookCallBackText);
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via Tremor via Age variable and 2 strings Dx06 and Dx35
        public void Pt8JumpViaTremorPathwayViaAgeVariableand2SetNodes()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Tremor", TestScenarioSex.Male, 13);

            var outcomePage = questionPage
                .AnswerNo()
                .Answer<OutcomePage>("Solvents");

            outcomePage.VerifyDispositionCode("Dx06");

            var genderPage = outcomePage.NavigateBackToGenderPage();

            genderPage.VerifyHeader();
            genderPage.SelectSexAndAge(TestScenarioSex.Male, 14);

            var searchpage = genderPage.NextPage();
            var questionInfoPage = searchpage.TypeSearchTextAndSelect("Tremor");
            var newQuestionPage = questionInfoPage.ClickIUnderstand();

            var newOutcomePage = newQuestionPage
                .AnswerNo()
                .Answer<OutcomePage>("Solvents");

            newOutcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 New jump to Dx11 in V15
        public void Pt8JumpViaTremorPathwayViaAgeVariabletoDx11()
        {
            var questionPage = TestScenarios.LaunchTriageScenario(Driver, "Behaviour Change", TestScenarioSex.Male, 24);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(3)
                .Answer(1)
                .Answer(3)
                .AnswerSuccessiveNo(9)
                .Answer<OutcomePage>("Yes");

            outcomePage.VerifyDispositionCode("Dx11");
        }
    }
}
