using System.Net;
using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class LocationPageTests : BaseTests
    {
       
        [Test]
        public void LocationPage_Displays()
        {
            var locationPage = GetLocationPage();
            locationPage.CompareAndVerify("1");
            Assert.True(locationPage.PostcodeFieldVisible());
        }
        
        [Test]
        public void ClickingNext_WithoutPostcode_ShowsValidation()
        {
            var submitPostcodeResult = GetLocationPage()
                .ClearPostcodeField()
                .ClickNext();

            Assert.True(submitPostcodeResult.ValidationVisible());
        }

        [Test]
        public void ClickingNext_WithPostcode_Redirects()
        {
            var submitPostcodeResult = GetLocationPage()
                .EnterPostcode(Postcodes.GetPathwaysPostcode())
                .ClickNext();

            Assert.IsAssignableFrom<ModuleZeroPage>(submitPostcodeResult);
        }

        [Test]
        public void EnteringOutOfAreaPostcode_RedirectsToOutOfArea()
        {
            var submitPostcodeResult = GetLocationPage()
                .EnterPostcode(Postcodes.GetOutOfAreaPostcode())
                .ClickNext();

            Assert.IsAssignableFrom<OutOfAreaPage>(submitPostcodeResult);
        }

        private LocationPage GetLocationPage()
        {
            var homePage = TestScenarioPart.HomePage(Driver);
            return TestScenarioPart.Location(homePage);
        }
    }
}
