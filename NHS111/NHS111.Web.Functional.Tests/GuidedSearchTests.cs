using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    public class GuidedSearchTests : BaseTests
    {
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Breathlessness", "PW556")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Breathlessness", "PW559")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Breathlessness", "PW557")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Breathlessness", "PW560")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Coldorflusymptoms", "PW1040")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Coldorflusymptoms", "PW1042")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Coldorflusymptoms", "PW1041")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Coldorflusymptoms", "PW1043")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Cough", "PW975")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Cough", "PW976")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Cough", "PW978")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Cough", "PW979")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Hightemperaturewithnoothersymptoms", "PW708")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Hightemperaturewithnoothersymptoms", "PW711")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Hightemperaturewithnoothersymptoms", "PW709")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Hightemperaturewithnoothersymptoms", "PW712")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Tiredness", "PW1070")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Tiredness", "PW1072")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Tiredness", "PW1071")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Tiredness", "PW1073")]
        public void GuidedSelectionTest_ViaCovidLinkVariousOptionsReturnExpectedPathway(string sex, int age, string guidedSelection, string expectedPathway)
        {
            var guidedSelectionPage = TestScenarios.LaunchGuidedSelectionScenario(Driver, sex, age);
            var weirdQuestionPage = guidedSelectionPage.GuidedSelection(guidedSelection);

            weirdQuestionPage.VerifyHiddenField("PathwayNo", expectedPathway);
        }


        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Breathlessness", "PW556")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Breathlessness", "PW559")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Breathlessness", "PW557")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Breathlessness", "PW560")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Coldorflusymptoms", "PW1040")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Coldorflusymptoms", "PW1042")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Coldorflusymptoms", "PW1041")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Coldorflusymptoms", "PW1043")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Cough", "PW975")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Cough", "PW976")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Cough", "PW978")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Cough", "PW979")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Hightemperaturewithnoothersymptoms", "PW708")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Hightemperaturewithnoothersymptoms", "PW711")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Hightemperaturewithnoothersymptoms", "PW709")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Hightemperaturewithnoothersymptoms", "PW712")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Lossorchangetoyoursenseofsmellortaste", "PW1854")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "Tiredness", "PW1070")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "Tiredness", "PW1072")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "Tiredness", "PW1071")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "Tiredness", "PW1073")]
        public void GuidedSelectionTest_ViaCovidSearchReturnExpectedPathway(string sex, int age, string guidedSelection, string expectedPathway)
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, sex, age);
            var guidedSelectionPage = searchPage.SearchByCovidTerm("covid");
            var weirdQuestionPage = guidedSelectionPage.GuidedSelection(guidedSelection);

            weirdQuestionPage.VerifyHiddenField("PathwayNo", expectedPathway);
        }


        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "noneofthese")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "noneofthese")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "noneofthese")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "noneofthese")]
        public void GuidedSelectionTest_ViaCovidLinkSelectNoneOfThese(string sex, int age, string guidedSelection)
        {
            var guidedSelectionPage = TestScenarios.LaunchGuidedSelectionScenario(Driver, sex, age);
            var explainerPage = guidedSelectionPage.GuidedSelection(guidedSelection);

            Assert.IsTrue(explainerPage.Driver.Title.Contains("NHS 111 Online - Coronavirus (COVID-19) symptoms"));
        }

        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Adult, "noneofthese")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Adult, "noneofthese")]
        [TestCase(TestScenarioSex.Female, TestScenarioAgeGroups.Child, "noneofthese")]
        [TestCase(TestScenarioSex.Male, TestScenarioAgeGroups.Child, "noneofthese")]
        public void GuidedSelectionTest_ViaCovidSearchSelectNoneOfThese(string sex, int age, string guidedSelection)
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, sex, age);
            var guidedSelectionPage = searchPage.SearchByCovidTerm("covid");
            var explainerPage = guidedSelectionPage.GuidedSelection(guidedSelection);

            Assert.IsTrue(explainerPage.Driver.Title.Contains("NHS 111 Online - Coronavirus (COVID-19) symptoms"));
        }
    }
}
