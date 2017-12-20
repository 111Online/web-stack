using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class HomePageTests : BaseTests
    {
        [Test]
        [Ignore]
        public void HomePage_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            homePage.Verify();
        }
    }
}
