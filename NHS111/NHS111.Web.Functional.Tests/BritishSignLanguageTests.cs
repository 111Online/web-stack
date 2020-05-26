using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class BritishSignLanguageTests : BaseTests
    {
        [Test]
        public void BritishSignLanguagePage_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var languagePage = homePage.ClickBritishSignLanguageLink();
            languagePage.Verify();
        }
    }
}
