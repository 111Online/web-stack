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

        //--Breathlessness-----------------------------------------

        [Test]
        public void NavigateViaCovidLinkToBreathlessness_PW556FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Breathlessness", "PW556");
        }

        [Test]
        public void NavigateViaCovidLinkToBreathlessness_PW559MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Breathlessness", "PW559");
        }

        [Test]
        public void NavigateViaCovidLinkToBreathlessness_PW557FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Breathlessness", "PW557");
        }

        [Test]
        public void NavigateViaCovidLinkToBreathlessness_PW560MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Breathlessness", "PW560");
        }

        //--Coldandflusymptoms-----------------------------------------

        [Test]
        public void NavigateViaCovidLinkToColdandflusymptoms_PW1040FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Coldandflusymptoms", "PW1040");
        }

        [Test]
        public void NavigateViaCovidLinkToColdandflusymptoms_PW1042MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Coldandflusymptoms", "PW1042");
        }

        [Test]
        public void NavigateViaCovidLinkToColdandflusymptoms_PW1041FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Coldandflusymptoms", "PW1041");
        }

        [Test]
        public void NavigateViaCovidLinkToColdandflusymptoms_PW1043MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Coldandflusymptoms", "PW1043");
        }

        //--Cough----------------------------

        [Test]
        public void NavigateViaCovidLinkToCough_PW975FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Cough", "PW975");
        }

        [Test]
        public void NavigateViaCovidLinkToCough_PW976MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Cough", "PW976");
        }

        [Test]
        public void NavigateViaCovidLinkToCough_PW978FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Cough", "PW978");
        }

        [Test]
        public void NavigateViaCovidLinkToCough_PW979MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Cough", "PW979");
        }

        //--Headache----------------------------

        [Test]
        public void NavigateViaCovidLinkToHeadache_PW752FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Headache", "PW752");
        }

        [Test]
        public void NavigateViaCovidLinkToHeadache_PW755MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Headache", "PW755");
        }

        [Test]
        public void NavigateViaCovidLinkToHeadache_PW753FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Headache", "PW753");
        }

        [Test]
        public void NavigateViaCovidLinkToHeadache_PW756MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Headache", "PW756");
        }

        //--Hightemperature(fever)----------------------------

        [Test]
        public void NavigateViaCovidLinkToHightemperaturefever_PW708FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Hightemperature(fever)", "PW708");
        }

        [Test]
        public void NavigateViaCovidLinkToHightemperaturefever_PW711MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Hightemperature(fever)", "PW711");
        }

        [Test]
        public void NavigateViaCovidLinkToHightemperaturefever_PW709FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Hightemperature(fever)", "PW709");
        }

        [Test]
        public void NavigateViaCovidLinkToHightemperaturefever_PW712MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Hightemperature(fever)", "PW712");
        }

        //--Lossorchangetoyoursenseofsmellortaste----------------------------

        [Test]
        public void NavigateViaCovidLinkToLossorchangetoyoursenseofsmellortaste_PW1854FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854");
        }

        [Test]
        public void NavigateViaCovidLinkToLossorchangetoyoursenseofsmellortaste_PW1854MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854");
        }

        [Test]
        public void NavigateViaCovidLinkToLossorchangetoyoursenseofsmellortaste_PW1854FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854");
        }

        [Test]
        public void NavigateViaCovidLinkToLossorchangetoyoursenseofsmellortaste_PW1854MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854");
        }

        //--Sorethroat----------------------------

        [Test]
        public void NavigateViaCovidLinkToSorethroat_PW854FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Sorethroat", "PW854");
        }

        [Test]
        public void NavigateViaCovidLinkToSorethroat_PW854MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Sorethroat", "PW854");
        }

        [Test]
        public void NavigateViaCovidLinkToSorethroat_PW854FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Sorethroat", "PW854");
        }

        [Test]
        public void NavigateViaCovidLinkToSorethroat_PW854MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Sorethroat", "PW854");
        }

        //--Tiredness-----------------------------------------

        [Test]
        public void NavigateViaCovidLinkToTiredness_PW1070FemaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Tiredness", "PW1070");
        }

        [Test]
        public void NavigateViaCovidLinkToTiredness_PW1072MaleAdult()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Tiredness", "PW1072");
        }

        [Test]
        public void NavigateViaCovidLinkToTiredness_PW1071FemaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Tiredness", "PW1071");
        }

        [Test]
        public void NavigateViaCovidLinkToTiredness_PW1073MaleChild()
        {
            GuidedSelectionTest(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Tiredness", "PW1073");
        }

    }
}
