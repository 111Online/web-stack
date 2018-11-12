using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class ServerErrorPageTests : BaseTests
    {
        [Test]
        public void ServerErrorPage_Displays()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            var serverErrorPage = searchPage.TypeErrorSearch();
            serverErrorPage.Verify();
        }
    }
}
