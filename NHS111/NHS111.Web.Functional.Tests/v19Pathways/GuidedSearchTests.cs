using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests.v19Pathways
{
    public class GuidedSearchTests : BaseTests
    {
        private QuestionPage LaunchViaCovidLink(string sex, int age)
        {
            var homepage = TestScenarioPart.HomePage(Driver);
            var covidHomePage = homepage.ClickCovidLink();
            var locationPage = covidHomePage.ClickOnStartNow();
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            return TestScenarioPart.Question(demographicsPage, sex, age);
        }

        private void GuidedSelectionTest(string sex, int age, string guidedSelection, string expectedPathway )
        {
            var guidedSelectionPage = LaunchViaCovidLink(sex, age);

            var weirdQuestionPage = guidedSelectionPage.guidedSelection(guidedSelection, true);

            weirdQuestionPage.VerifyHiddenField("PathwayNo", expectedPathway);
        }

        //--Cough----------------------------

        [Test]
        public void NavigateToCoughPW975FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Cough", "PW975");
        }

        [Test]
        public void NavigateToCoughPW976MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Cough", "PW976");
        }

        [Test]
        public void NavigateToCoughPW978FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Cough", "PW978");
        }

        [Test]
        public void NavigateToCoughPW979MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Cough", "PW979");
        }

        //--Coldandflusymptoms-----------------------------------------

        [Test]
        public void NavigateToColdandflusymptomsPW975FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Coldandflusymptoms", "PW1040");
        }

        [Test]
        public void NavigateToColdandflusymptomsPW976MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Coldandflusymptoms", "PW1042");
        }

        [Test]
        public void NavigateToColdandflusymptomsPW978FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Coldandflusymptoms", "PW1041");
        }

        [Test]
        public void NavigateToColdandflusymptomsPW979MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Coldandflusymptoms", "PW1043");
        }

    }
}
