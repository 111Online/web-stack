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
        public void ReferRingAndGoPharmacistService()
        {
            var questionPage = TestScenerios.LaunchRecommendedServiceScenerio(Driver, "Emergency Prescription 111 Online", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "L12SA");

            questionPage.VerifyQuestion("Can you get in touch with your GP or usual pharmacy?");
            var recommendedServicePage = questionPage
                .Answer(2)
                .Answer<OutcomePage>(1)
                .ClickShowServices();

            recommendedServicePage.VerifyServiceDetails();
            recommendedServicePage.VerifyOtherServices();
            recommendedServicePage.CompareAndVerify("1");
        }
    }
}
