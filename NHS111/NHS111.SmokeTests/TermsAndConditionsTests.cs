using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class TermsAndConditionsTests : BaseTests
    {
        [Test]
        public void TermsAndConditionsPage_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var termsPage = homePage.ClickTermsLink();
            termsPage.Verify();
        }
    }
}
