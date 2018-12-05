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
