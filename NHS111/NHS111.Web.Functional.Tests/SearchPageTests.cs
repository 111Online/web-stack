using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
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
            var questionInfoPage = searchPage.TypeSearchTextAndSelect("Bites and Stings");
            var questionPage = questionInfoPage.ClickIUnderstand();
            questionPage.VerifyQuestionPageLoaded();
        }

        [Test]
        public void SearchPage_TabbingOrder()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.VerifyTabbingOrder("Bites or Stings");
        }

        [Test]
        [Ignore("Currently there is no validation on empty values. Ignoring unti confirmed this is correct.")]
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
            searchPage.VerifyTermHits("Headache or migraine", 1);
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
