using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class CategoryPageTests : BaseTests
    {
        [Test]
        public void CategoryPage_Displays()
        {
            var categoryPage = TestScenerios.LaunchCategoryScenerio(Driver, "Male", 30);
            categoryPage.VerifyHeader();
        }

        [Test]
        public void CategoryPage_CategoriesShownWhenNoSearchResults()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            var categoryPage = searchPage.TypeInvalidSearch();
            categoryPage.VerifyNoResultsMessage(searchPage._invalidSearchText);
        }

        [Test]
        public void CategoryPage_TabbingOrder()
        {
            var categoryPage = TestScenerios.LaunchCategoryScenerio(Driver, "Male", 30);
            categoryPage.VerifyTabbingOrder("Head and neck", "Ear", "Blocked ear");
        }
    }
}
