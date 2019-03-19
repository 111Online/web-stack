using NHS111.Web.Functional.Utils;
using NHS111.Web.Functional.Utils.ScreenShot;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    [ScreenShotComparison]
    public class RecommendedServiceTests : BaseTests
    {
        [Test]
        public void NumsasPharmacistService()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Emergency Prescription", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            questionPage.VerifyQuestion("Can you get in touch with your GP or usual pharmacy?");
            var recommendedServicePage = questionPage
                .Answer(2)
                .Answer(2)
                .Answer<OutcomePage>(1)
                .ClickShowServices();

            recommendedServicePage.VerifyCallout();
            recommendedServicePage.VerifyServiceDetails();
            recommendedServicePage.VerifyOtherServices();
            recommendedServicePage.CompareAndVerify("1");
        }
    }
}
