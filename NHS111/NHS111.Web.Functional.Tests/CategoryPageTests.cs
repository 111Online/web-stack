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
            categoryPage.VerifyTabbingOrder("Accidentally taking something harmful");
        }

        /*
         * This test will rely on services being in the Pharmacy Whitelist from CCG Service
         */
        [TestCase]
        public void CategoryPage_EmergencyPrescriptionsPilotArea()
        {
            // First check a postcode that should show EP does
            var categoryPilot = TestScenerios.LaunchCategoryScenerio(Driver, TestScenerioSex.Female, 40, "LS17 7NZ");
            categoryPilot.VerifyCategoryNotExists("Emergency prescriptions");
            
            // Then check a postcode that shouldn't show EP doesn't
            var categoryNotPilot = TestScenerios.LaunchCategoryScenerio(Driver, TestScenerioSex.Female, 40, "PO22 8PB");
            categoryNotPilot.VerifyCategoryNotExists("Emergency prescriptions");
        }
    }
}
