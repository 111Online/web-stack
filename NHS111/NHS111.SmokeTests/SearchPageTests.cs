using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class SearchPageTests : BaseTests
    {
        [Test]
        public void SearchPage_Displays()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 30);
            searchPage.Verify();
        }
    }
}
