using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class ModuleZeroTests : BaseTests
    {
        [Test]
        public void ModuleZero_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var moduleZero = TestScenarioPart.ModuleZero(homePage);
            moduleZero.VerifyHeader();
        }

        [Test]
        public void ModuleZero_ExpandableLink()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var moduleZero = TestScenarioPart.ModuleZero(homePage);
            moduleZero.VerifyExpandableLink();
        }


    }
}
