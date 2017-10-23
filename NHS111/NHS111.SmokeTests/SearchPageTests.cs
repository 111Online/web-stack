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
            const string pathwayTitle = "Bites and Stings";
            var questionPage = searchPage.TypeSearchTextAndSelect(pathwayTitle);
            questionPage.VerifyQuestionPageLoaded();
        }

        [Test]
        public void SearchPage_TabbingOrder()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.VerifyTabbingOrder("Bites and Stings");
        }

        [Test]
        public void SearchPage_NoReultsValidation()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            const string noResultsSearchTerm = "g0bb13dyg00k";
            searchPage.SearchByTerm(noResultsSearchTerm);
            searchPage.VerifyNoResultsValidaion(noResultsSearchTerm);
        }
    }
}
