using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    public class GuidedSearchTests : BaseTests
    {
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Breathlessness", "PW556")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Breathlessness", "PW559")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Breathlessness", "PW557")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Breathlessness", "PW560")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Coldandflusymptoms", "PW1040")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Coldandflusymptoms", "PW1042")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Coldandflusymptoms", "PW1041")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Coldandflusymptoms", "PW1043")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Cough", "PW975")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Cough", "PW976")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Cough", "PW978")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Cough", "PW979")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Headache", "PW752")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Headache", "PW755")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Headache", "PW753")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Headache", "PW756")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Hightemperature(fever)", "PW708")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Hightemperature(fever)", "PW711")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Hightemperature(fever)", "PW709")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Hightemperature(fever)", "PW712")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Sorethroat", "PW854")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Sorethroat", "PW854")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Sorethroat", "PW854")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Sorethroat", "PW854")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Tiredness", "PW1070")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Tiredness", "PW1072")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Tiredness", "PW1071")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Tiredness", "PW1073")]
        public void GuidedSelectionTest_ViaCovidLinkVariousOptionsReturnExpectedPathway(string sex, int age, string guidedSelection, string expectedPathway)
        {
            var guidedSelectionPage = TestScenerios.LaunchGuidedSelectionScenario(Driver, sex, age);
            var weirdQuestionPage = guidedSelectionPage.GuidedSelection(guidedSelection);

            weirdQuestionPage.VerifyHiddenField("PathwayNo", expectedPathway);
        }


        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Breathlessness", "PW556")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Breathlessness", "PW559")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Breathlessness", "PW557")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Breathlessness", "PW560")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Coldandflusymptoms", "PW1040")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Coldandflusymptoms", "PW1042")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Coldandflusymptoms", "PW1041")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Coldandflusymptoms", "PW1043")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Cough", "PW975")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Cough", "PW976")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Cough", "PW978")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Cough", "PW979")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Headache", "PW752")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Headache", "PW755")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Headache", "PW753")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Headache", "PW756")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Hightemperature(fever)", "PW708")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Hightemperature(fever)", "PW711")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Hightemperature(fever)", "PW709")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Hightemperature(fever)", "PW712")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Sorethroat", "PW854")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Sorethroat", "PW854")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Sorethroat", "PW854")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Sorethroat", "PW854")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "Tiredness", "PW1070")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "Tiredness", "PW1072")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "Tiredness", "PW1071")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "Tiredness", "PW1073")]
        public void GuidedSelectionTest_ViaCovidSearchReturnExpectedPathway(string sex, int age, string guidedSelection, string expectedPathway)
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, sex, age);
            var guidedSelectionPage = searchPage.SearchByCovidTerm("covid");
            var weirdQuestionPage = guidedSelectionPage.GuidedSelection(guidedSelection);

            weirdQuestionPage.VerifyHiddenField("PathwayNo", expectedPathway);
        }


        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "noneofthese")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "noneofthese")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "noneofthese")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "noneofthese")]
        public void GuidedSelectionTest_ViaCovidLinkSelectNoneOfThese(string sex, int age, string guidedSelection)
        {
            var guidedSelectionPage = TestScenerios.LaunchGuidedSelectionScenario(Driver, sex, age);
            var explainerPage = guidedSelectionPage.GuidedSelection(guidedSelection);

            Assert.IsTrue(explainerPage.Driver.Title.Contains("NHS 111 Online - Coronavirus (COVID-19) symptoms"));
        }

        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "noneofthese")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "noneofthese")]
        [TestCase(TestScenerioSex.Female, TestScenerioAgeGroups.Child, "noneofthese")]
        [TestCase(TestScenerioSex.Male, TestScenerioAgeGroups.Child, "noneofthese")]
        public void GuidedSelectionTest_ViaCovidSearchSelectNoneOfThese(string sex, int age, string guidedSelection)
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, sex, age);
            var guidedSelectionPage = searchPage.SearchByCovidTerm("covid");
            var explainerPage = guidedSelectionPage.GuidedSelection(guidedSelection);

            Assert.IsTrue(explainerPage.Driver.Title.Contains("NHS 111 Online - Coronavirus (COVID-19) symptoms"));
        }
    }
}
