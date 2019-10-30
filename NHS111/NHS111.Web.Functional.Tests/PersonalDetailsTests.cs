using NHS111.Web.Functional.Utils;
using NHS111.Web.Functional.Utils.ScreenShot;

namespace NHS111.Web.Functional.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using OpenQA.Selenium;

    [TestFixture]
    class PersonalDetailsTests : BaseTests
    {
        [Test]
        public void PersonalDetailsViaEP()
        {
            var questionPage = TestScenerios.LaunchRecommendedServiceScenerio(Driver, "Emergency Prescription 111 online", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "PR67JY");

            questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
            var recommendedServicePage = questionPage
                .Answer(2)
                .Answer<PreOutcomePage>(1)
                .ClickShowServices();
            var referralExplanationPage = recommendedServicePage.ClickActionLink();
            var personalDetailsPage = referralExplanationPage.ClickButton();

            personalDetailsPage.VerifyIsPersonalDetailsPage();
        }


        [Test]
        public void PersonalDetailsFirstPartyAtHome()
        {
            var personalDetailsPage = TestScenerios.LaunchPersonalDetailsScenario(Driver, "Diabetes Blood Sugar Problem (Declared)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "sk10 3de");

            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyNumberDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Test1", "Tester1");

            personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            personalDetailsPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = personalDetailsPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "22180492"; // Belong Macclesfield, Kennedy Avenue, Macclesfield, Cheshire, SK10 3DE
            currentAddressPage.VerifyAddressDisplays(addressID);
            
            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Book your call");
            confirmDetails.VerifyThirdPartyBannerNotDisplayed();
        }


        [Test]
        public void PersonalDetailsThirdPartyAtHome()
        {
            var personalDetailsPage = TestScenerios.LaunchPersonalDetailsScenario(Driver, "Diabetes Blood Sugar Problem (Declared)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "sk10 3de");

            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyNumberDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            personalDetailsPage.SelectSomeoneElse();
            personalDetailsPage.EnterPatientName("Test1", "Tester1");
            personalDetailsPage.EnterThirdPartyName("Test2", "Tester2");
            personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            personalDetailsPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = personalDetailsPage.SubmitPersonalDetails();

            currentAddressPage.VerifyHeading("Where are they right now?");

            var addressID = "22180498"; // 60 Kennedy Avenue, Macclesfield, Cheshire, SK10 3DE
            currentAddressPage.VerifyAddressDisplays(addressID);
            
            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are they at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Book your call");
            confirmDetails.VerifyThirdPartyBannerIsDisplayed();
        }


        [Test]
        public void PersonalDetailsFirstPartyNotAtHome()
        {
            var personalDetailsPage = TestScenerios.LaunchPersonalDetailsScenario(Driver, "Diabetes Blood Sugar Problem (Declared)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "sk10 3de");

            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyNumberDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Test1", "Tester1");
            personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            personalDetailsPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = personalDetailsPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "22180497"; // 109 Kennedy Avenue, Macclesfield, Cheshire, SK10 3DE
            currentAddressPage.VerifyAddressDisplays(addressID);
            
            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeNo();

            var homePostcodePage = atHomePage.SubmitAtHome();
            homePostcodePage.TypeHomePostcode("LS17 7NZ");
            var confirmDetails = homePostcodePage.SubmitHomePostcode();
            confirmDetails.VerifyHeading("Book your call");
            confirmDetails.VerifyThirdPartyBannerNotDisplayed();
        }


        [Test]
        public void PersonalDetailsThirdPartyNotAtHome()
        {
            var personalDetailsPage = TestScenerios.LaunchPersonalDetailsScenario(Driver, "Diabetes Blood Sugar Problem (Declared)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "sk10 3de");


            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyNumberDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            personalDetailsPage.SelectSomeoneElse();
            personalDetailsPage.EnterPatientName("Test1", "Tester1");
            personalDetailsPage.EnterThirdPartyName("Test2", "Tester2");
            personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            personalDetailsPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = personalDetailsPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are they right now?");

            var addressID = "22180496"; // 107 Kennedy Avenue, Macclesfield, Cheshire, SK10 3DE
            currentAddressPage.VerifyAddressDisplays(addressID);
            
            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are they at home?");
            atHomePage.SelectAtHomeNo();
            
            var homePostcodePage = atHomePage.SubmitAtHome();
            homePostcodePage.TypeHomePostcode("LS17 7NZ");
            var confirmDetails = homePostcodePage.SubmitHomePostcode();
            confirmDetails.VerifyHeading("Book your call");
            confirmDetails.VerifyThirdPartyBannerIsDisplayed();
        }

        [Test]
        public void PersonalDetailsAddressNotListed()
        {
            var personalDetailsPage = TestScenerios.LaunchPersonalDetailsScenario(Driver, "Diabetes Blood Sugar Problem (Declared)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "sk10 3de");

            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyNumberDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            personalDetailsPage.SelectSomeoneElse();
            personalDetailsPage.EnterPatientName("Test1", "Tester1");
            personalDetailsPage.EnterThirdPartyName("Test2", "Tester2");
            personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            personalDetailsPage.EnterPhoneNumber("07793346301");

            var currentAddressPage = personalDetailsPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are they right now?");
            
            var checkAddressPage = currentAddressPage.ClickAddressNotListed();
            checkAddressPage.VerifyHeading("Check address again");
        }
    }
}
