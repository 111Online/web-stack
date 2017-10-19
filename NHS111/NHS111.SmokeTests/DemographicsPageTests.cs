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
            GetDemographicsPage().VerifyHeader();
        }

        [Test]
        public void DemographicsPage_NumberInputOnly()
        {
            GetDemographicsPage().VerifyAgeInputBox(TestScenerioGender.Male, "25INVALIDTEXT!£$%^&*()_+{}:@~>?</*-+");
        }

        [Test]
        public void DemographicsPage_AgeTooOldShowsValidation()
        {
            GetDemographicsPage().VerifyTooOldAgeShowsValidation(TestScenerioGender.Male, 201);
        }

        [Test]
        public void DemographicsPage_AgeTooYoungShowsValidation()
        {
            GetDemographicsPage().VerifyTooYoungAgeShowsValidation(TestScenerioGender.Male, 4);
        }

        private DemographicsPage GetDemographicsPage()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var moduleZero = TestScenarioPart.ModuleZero(homePage);
            return TestScenarioPart.Demographics(moduleZero);
        }
    }
}
