using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class ServerErrorPageTests : BaseTests
    {
        [Test]
        [Category("PreLive")]
        public void ServerErrorPage_Displays()
        {
            var searchPage = TestScenarios.LaunchSearchScenario(Driver, TestScenarioSex.Male, 30);
            var serverErrorPage = searchPage.TypeErrorSearch();
            serverErrorPage.Verify();
        }
    }
}
