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
            categoryPage.Verify();
        }
    }
}
