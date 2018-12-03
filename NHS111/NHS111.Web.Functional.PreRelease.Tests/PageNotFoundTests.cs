using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.PreRelease.Tests
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
