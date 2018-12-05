using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
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
            categoryPage.VerifyNoResultsMessage(searchPage.InvalidSearchText);
        }

        [Test]
        public void CategoryPage_TabbingOrder()
        {
            var categoryPage = TestScenerios.LaunchCategoryScenerio(Driver, "Male", 30);
            categoryPage.VerifyTabbingOrder("Accidental overdose or taking something harmful");
        }
    }
}
