using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
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
