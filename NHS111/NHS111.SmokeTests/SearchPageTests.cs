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
            searchPage.VerifyHeader();
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
        public void SearchPage_NoInputValidation()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.SearchByTerm(string.Empty);
            searchPage.VerifyNoInputValidation();
        }

        [Test]
        public void SearchPage_ResultsWithApostropheHyphenAndBrackets()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.SearchByTerm("'-/)}]Headache[{(\\");
            searchPage.VerifyTermHits("Headache and migraine", 1);
        }

        [Test]
        public void SearchPage_CategoryLinkShowsWithSearchResults()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.SearchByTerm("Headache");
            searchPage.VerifyCategoriesLinkPresent();
        }
    }
}
