using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class PageNotFoundTests : BaseTests
    {
        [Test]
        public void PageNotFound_Displays()
        {
            TestScenarioPart.HomePage(Driver);


            var pageNotFound = TestScenarioPart.PageNotFound(Driver);
            
            pageNotFound.Verify();
        }
    }
}
