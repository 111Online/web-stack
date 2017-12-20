using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class ModuleZeroTests : BaseTests
    {
        [Test]
        public void ModuleZeroPage_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var moduleZero = TestScenarioPart.ModuleZero(homePage);
            moduleZero.VerifyHeader();
        }

        [Test]
        public void ModuleZeroPage_List()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var moduleZero = TestScenarioPart.ModuleZero(homePage);
            moduleZero.VerifyList();
        }


    }
}
