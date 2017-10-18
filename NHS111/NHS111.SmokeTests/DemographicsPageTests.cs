using NUnit.Framework;
using NHS111.SmokeTest.Utils;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class DemographicsPageTests : BaseTests
    {
        [Test]
        public void DemographicsPage_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var moduleZero = TestScenarioPart.ModuleZero(homePage);
            var demographics = TestScenarioPart.Demographics(moduleZero);

            demographics.Verify();
        }
    }
}
