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
            var moduleZero = TestScenarioPart.ModuleZero(Driver);
            moduleZero.VerifyHeader();
        }

        [Test]
        public void ModuleZeroPage_List()
        {
            var moduleZero = TestScenarioPart.ModuleZero(Driver);
            moduleZero.VerifyList();
        }


    }
}
