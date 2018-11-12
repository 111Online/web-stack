using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class CookiesStatementTests : BaseTests
    {
        [Test]
        public void CookiesStatementPage_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var cookiesPage = homePage.ClickCookiesStatementLink();
            cookiesPage.Verify();
        }
    }
}
