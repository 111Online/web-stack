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
            GetDemographicsPage().VerifyAgeInputBox(TestScenerioSex.Male, "25INVALIDTEXT!£$%^&*()_+{}:@~>?</*-+");
        }

        [Test]
        public void DemographicsPage_AgeTooOldShowsValidation()
        {
            GetDemographicsPage().VerifyTooOldAgeShowsValidation(TestScenerioSex.Male, 201);
        }

        [Test]
        public void DemographicsPage_AgeTooYoungShowsValidation()
        {
            GetDemographicsPage().VerifyTooYoungAgeShowsValidation(TestScenerioSex.Male, 4);
        }

        [Test]
        public void DemographicsPage_NoSexSelectionShowsValidation()
        {
            GetDemographicsPage().VerifyNoSexValidation(20);
        }

        [Test]
        public void DemographicsPage_NoAgeEnteredShowsValidation()
        {
            GetDemographicsPage().VerifyNoAgeValidation(TestScenerioSex.Male);
        }

        [Test]
        public void DemographicsPage_TabbingOrder()
        {
            GetDemographicsPage().VerifyTabbingOrder(40);
        }

        private DemographicsPage GetDemographicsPage()
        {
            var moduleZero = TestScenarioPart.ModuleZero(Driver);
            return TestScenarioPart.Demographics(moduleZero);
        }
    }
}
