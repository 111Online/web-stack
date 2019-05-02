using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class ModuleZeroTests : BaseTests
    {
        [Test]
        public void ModuleZeroPage_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZero = TestScenarioPart.ModuleZero(locationPage);
            moduleZero.VerifyHeader();
        }

        [Test]
        public void ModuleZeroPage_List()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZero = TestScenarioPart.ModuleZero(locationPage);
            moduleZero.VerifyList();
        }


    }
}
