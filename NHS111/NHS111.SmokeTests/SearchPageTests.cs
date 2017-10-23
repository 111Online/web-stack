using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class SearchPageTests : BaseTests
    {
        [Test]
        public void SearchPage_Displays()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.Verify();
        }

        [Test]
        public void SearchPage_SelectFirstResultStartsPathway()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            var questionPage = searchPage.TypeSearchTextAndSelect("Bites and Stings");
            questionPage.VerifyQuestionPageLoaded();
        }

        [Test]
        public void SearchPage_TabbingOrder()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.VerifyTabbingOrder("Bites and Stings");
        }

        [Test]
        public void SearchPage_NoResultsValidation()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            const string noResultsSearchTerm = "g0bb13dyg00k";
            searchPage.SearchByTerm(noResultsSearchTerm);
            searchPage.VerifyNoResultsValidation(noResultsSearchTerm);
        }

        [Test]
        public void SearchPage_NoInputValidation()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.SearchByTerm(string.Empty);
            searchPage.VerifyNoInputValidation();
        }

        [Test]
        public void SearchPage_ResultsEvenWithApostropheHyphenAndBrackets()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.SearchByTerm("'-/)}]Headache[{(\\");
            searchPage.VerifyTermHits("Headache and migraine", 1);
        }

    }
}
