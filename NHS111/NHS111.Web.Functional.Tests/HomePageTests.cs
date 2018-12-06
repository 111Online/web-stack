using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class HomePageTests : BaseTests
    {
        [Test]
        [Ignore]
        public void HomePage_Displays()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            homePage.Verify();
        }

        [Test]
        public void HomePage_Displays_with_Headers_using_default_url()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            homePage.Verify();
            homePage.VerifyHeaderBannerDisplayed();
        }

        [Test]
        public void HomePage_Displays_without_Headers_using_app_url()
        {
            var homePage = TestScenarioPart.HomePage(Driver, "nhs app");
            homePage.VerifyHeaderBannerHidden();
        }


        [Test]
        public void HomePage_Displays_without_Headers_after_new_request()
        {
            var homePage = TestScenarioPart.HomePage(Driver, "nhs app");
            homePage.VerifyHeaderBannerHidden();
            homePage.Load();
            homePage.VerifyHeaderBannerHidden();
        }


        [Test]
        public void HomePage_Displays_with_Headers_after_new_request_using_non_app_url()
        {
            var homePage = TestScenarioPart.HomePage(Driver, "nhs app");
            homePage.VerifyHeaderBannerHidden();
            homePage.Load("direct");
            homePage.VerifyHeaderBannerDisplayed();
        }


        [Test]
        public void NextPage_Displays_without_Headers_following_request_using_app_urll()
        {
            var homePage = TestScenarioPart.HomePage(Driver, "nhs app");
            homePage.VerifyHeaderBannerHidden();
            var demographicsPage = homePage.ClickNextButton();
            demographicsPage.VerifyHeaderBannerHidden();

        }
    }
}
