using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
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
            GetDemographicsPage().VerifyAgeInputBox(TestScenarioSex.Male, "25INVALIDTEXT!£$%^&*()_+{}:@~>?</*-+");
        }

        [Test]
        public void DemographicsPage_AgeTooOldShowsValidation()
        {
            GetDemographicsPage().VerifyTooOldAgeShowsValidation(TestScenarioSex.Male, 201);
        }

        [Test]
        public void DemographicsPage_AgeTooYoungShowsValidation()
        {
            GetDemographicsPage().VerifyTooYoungAgeShowsValidation(TestScenarioSex.Male, 4);
        }

        [Test]
        public void DemographicsPage_NoSexSelectionShowsValidation()
        {
            GetDemographicsPage().VerifyNoSexValidation(20);
        }

        [Test]
        public void DemographicsPage_NoAgeEnteredShowsValidation()
        {
            GetDemographicsPage().VerifyNoAgeValidation(TestScenarioSex.Male);
        }

        [Test]
        public void DemographicsPage_TabbingOrder()
        {
            GetDemographicsPage().VerifyTabbingOrder(40);
        }

        private DemographicsPage GetDemographicsPage()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZero = TestScenarioPart.ModuleZero(locationPage);
            return TestScenarioPart.Demographics(moduleZero);
        }
    }
}
