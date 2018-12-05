using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
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
