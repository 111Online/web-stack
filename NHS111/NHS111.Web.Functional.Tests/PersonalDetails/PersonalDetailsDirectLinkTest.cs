using NHS111.Web.Functional.Utils;
using NUnit.Framework;
using System.Configuration;

namespace NHS111.Web.Functional.Tests.PersonalDetails
{
    [Category("UsingDirectLinkingApp")]
    [TestFixture]
    public class PersonalDetailsDirectLinkTest : BaseTests
    {
        private PersonalDetailsPage personalDetailsPage;

        [SetUp]
        public void LaunchPersonalDetailsPage()
        {
            new DirectLinkAppJumpToPersonalDetails(Driver)
                .SetOutcomeGroupId("ITK_Clinician_call_back")
                .SetPostcode("sk10 3de")
                .Navigate(ConfigurationManager.AppSettings["TestWebsiteUrl"]);

            personalDetailsPage = new PersonalDetailsPage(Driver);
        }

        [Test]
        public void PersonalDetailsFirstPartyAtHome()
        {
            personalDetailsPage.SelectMe();

            var yourNamePage = personalDetailsPage.SubmitInformantDetails();
            yourNamePage.EnterYourName("Test1", "Tester1");

            var dateofBirthPage = yourNamePage.SubmitYourNameDetails();
            dateofBirthPage.VerifyDateOfBirthDisplayed();
            dateofBirthPage.EnterDateOfBirth("31", "07", "1980");

            var telephoneNumberPage = dateofBirthPage.SubmitDateOfBirth();
            telephoneNumberPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = telephoneNumberPage.SubmitTelephoneNumber();
            //currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "22180492"; // Belong Macclesfield, Kennedy Avenue, Macclesfield, Cheshire, SK10 3DE
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            confirmDetails.VerifyThirdPartyBannerNotDisplayed();
        }

        [Test]
        public void PersonalDetailsThirdPartyAtHome()
        {
            personalDetailsPage.SelectSomeoneElse();

            var yourNamePage = personalDetailsPage.SubmitInformantDetails();
            yourNamePage.EnterYourName("Test1", "Tester1");

            var theirNamePage = yourNamePage.SubmitYourNameAsInformant();
            theirNamePage.EnterTheirName("Test2", "Tester2");

            var dateofBirthPage = theirNamePage.SubmitTheirNameDetails();
            dateofBirthPage.VerifyDateOfBirthDisplayed();
            dateofBirthPage.EnterDateOfBirth("31", "07", "1980");

            var telephoneNumberPage = dateofBirthPage.SubmitDateOfBirth();
            telephoneNumberPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = telephoneNumberPage.SubmitTelephoneNumber();

            currentAddressPage.VerifyHeading("Where are they right now?");

            var addressID = "22180498"; // 60 Kennedy Avenue, Macclesfield, Cheshire, SK10 3DE
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are they at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            confirmDetails.VerifyThirdPartyBannerIsDisplayed();
        }

        [Test]
        public void PersonalDetailsFirstPartyNotAtHome()
        {
            personalDetailsPage.SelectMe();
            var yourNamePage = personalDetailsPage.SubmitInformantDetails();
            yourNamePage.EnterYourName("Test1", "Tester1");

            var dateofBirthPage = yourNamePage.SubmitYourNameDetails();
            dateofBirthPage.VerifyDateOfBirthDisplayed();
            dateofBirthPage.EnterDateOfBirth("31", "07", "1980");

            var telephoneNumberPage = dateofBirthPage.SubmitDateOfBirth();
            telephoneNumberPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = telephoneNumberPage.SubmitTelephoneNumber();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "22180497"; // 109 Kennedy Avenue, Macclesfield, Cheshire, SK10 3DE
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeNo();

            var homePostcodePage = atHomePage.SubmitAtHome();
            homePostcodePage.TypeHomePostcode("LS17 7NZ");
            var confirmDetails = homePostcodePage.SubmitHomePostcode();
            confirmDetails.VerifyHeading("Check details");
            confirmDetails.VerifyThirdPartyBannerNotDisplayed();
            confirmDetails.VerifyConfirmNameDisplayed("Test1", "Tester1");
        }

        [Test]
        public void PersonalDetailsThirdPartyNotAtHome()
        {
            personalDetailsPage.SelectSomeoneElse();
            var yourNamePage = personalDetailsPage.SubmitInformantDetails();
            yourNamePage.EnterYourName("Test1", "Tester1");

            var theirNamePage = yourNamePage.SubmitYourNameAsInformant();
            theirNamePage.EnterTheirName("Test2", "Tester2");

            var dateofBirthPage = theirNamePage.SubmitTheirNameDetails();
            dateofBirthPage.VerifyDateOfBirthDisplayed();
            dateofBirthPage.EnterDateOfBirth("31", "07", "1980");

            var telephoneNumberPage = dateofBirthPage.SubmitDateOfBirth();
            telephoneNumberPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = telephoneNumberPage.SubmitTelephoneNumber();
            currentAddressPage.VerifyHeading("Where are they right now?");

            var addressID = "22180496"; // 107 Kennedy Avenue, Macclesfield, Cheshire, SK10 3DE
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are they at home?");
            atHomePage.SelectAtHomeNo();

            var homePostcodePage = atHomePage.SubmitAtHome();
            homePostcodePage.TypeHomePostcode("LS17 7NZ");
            var confirmDetails = homePostcodePage.SubmitHomePostcode();
            confirmDetails.VerifyHeading("Check details");
            confirmDetails.VerifyThirdPartyBannerIsDisplayed();
        }

        [Test]
        public void PersonalDetailsAddressNotListed()
        {
            personalDetailsPage.SelectSomeoneElse();
            var yourNamePage = personalDetailsPage.SubmitInformantDetails();
            yourNamePage.EnterYourName("Test1", "Tester1");

            var theirNamePage = yourNamePage.SubmitYourNameAsInformant();
            theirNamePage.EnterTheirName("Test2", "Tester2");

            var dateofBirthPage = theirNamePage.SubmitTheirNameDetails();
            dateofBirthPage.VerifyDateOfBirthDisplayed();
            dateofBirthPage.EnterDateOfBirth("31", "07", "1980");


            var telephoneNumberPage = dateofBirthPage.SubmitDateOfBirth();
            telephoneNumberPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = telephoneNumberPage.SubmitTelephoneNumber();
            currentAddressPage.VerifyHeading("Where are they right now?");

            var checkAddressPage = currentAddressPage.ClickAddressNotListed();
            checkAddressPage.VerifyHeading("Check address again");
        }
    }
}
