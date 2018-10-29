using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
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
