using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class PrivacyStatementTests : BaseTests
    {
        [Test]
        public void PrivacyStatementPage_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var privacyStatementPage = homePage.ClickPrivacyStatementLink();
            privacyStatementPage.Verify();
        }
    }
}
