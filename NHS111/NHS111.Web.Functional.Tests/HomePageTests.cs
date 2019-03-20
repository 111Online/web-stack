using System.Net;
using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class HomePageTests : BaseTests
    {
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
            homePage.Visit();
            homePage.VerifyHeaderBannerHidden();
        }


        [Test]
        public void HomePage_Displays_with_Headers_after_new_request_using_non_app_url()
        {
            var homePage = TestScenarioPart.HomePage(Driver, "nhs app");
            homePage.VerifyHeaderBannerHidden();
            homePage.Visit("direct");
            homePage.VerifyHeaderBannerDisplayed();
        }


        [Test]
        public void NextPage_Displays_without_Headers_following_request_using_app_url()
        {
            var homePage = HomePage.Start(Driver)
                .Visit("nhs app");
            homePage.VerifyHeaderBannerHidden();

            var moduleZeroPage = homePage.EnterPostcode(Postcodes.GetPathwaysPostcode())
                .ClickNext() as ModuleZeroPage;
            moduleZeroPage.VerifyHeaderBannerHidden();
        }

        [Test]
        public void ClickingNext_WithoutPostcode_ShowsValidation()
        {
            var submitPostcodeResult = HomePage.Start(Driver)
                .Visit()
                .ClearPostcodeField()
                .ClickNext();

            Assert.True(submitPostcodeResult.ValidationVisible());
        }

        [Test]
        public void ClickingNext_WithPostcode_Redirects()
        {
            var submitPostcodeResult = HomePage.Start(Driver)
                .Visit()
                .EnterPostcode(Postcodes.GetPathwaysPostcode())
                .ClickNext();

            Assert.IsAssignableFrom<ModuleZeroPage>(submitPostcodeResult);
        }

      

        [Test]
        public void EnteringOutOfAreaPostcode_RedirectsToOutOfArea()
        {
            var submitPostcodeResult = HomePage.Start(Driver)
                .Visit()
                .EnterPostcode(Postcodes.GetOutOfAreaPostcode())
                .ClickNext();

            Assert.IsAssignableFrom<OutOfAreaPage>(submitPostcodeResult);
        }
    }
}
