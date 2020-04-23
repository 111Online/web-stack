using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class PageNotFoundTests : BaseTests
    {
        [Test]
        [Category("PreLive")]
        public void PageNotFound_Displays()
        {
            var pageNotFound = TestScenarioPart.PageNotFound(Driver);

            pageNotFound.Verify();
        }
    }
}