using NHS111.Web.Functional.Utils;
using NHS111.Web.Functional.Utils.ScreenShot;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using OpenQA.Selenium;
 
    [TestFixture]
    public class ConfirmationScreenTests : BaseTests {

      [Test]
        public void ConfirmationscreensGP()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "E173AX");

            questionPage.VerifyQuestion("Have you hurt or banged your head in the last 4 weeks?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .Answer(1)
                .Answer(2)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(1)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyOutcome("Contact your GP now");

            //need to click 'I can't get an appointment today link
            //need to select DoS ID 2000006999

            //var personalDetailsPage = //??

                    
            //personalDetailsPage.VerifyIsPersonalDetailsPage();
            //personalDetailsPage.VerifyNameDisplayed();
            //personalDetailsPage.VerifyNumberDisplayed();
            //personalDetailsPage.VerifyDateOfBirthDisplayed();

            //personalDetailsPage.SelectMe();
            //personalDetailsPage.EnterPatientName("Test1", "Tester1");

            //personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            //personalDetailsPage.EnterPhoneNumber("07793346301");

            //var currentAddressPage = personalDetailsPage.SubmitPersonalDetails();
            //currentAddressPage.VerifyHeading("Where are you right now?");

            //var addressID = "55629068"; 
            //currentAddressPage.VerifyAddressDisplays(addressID);

            //var atHomePage = currentAddressPage.ClickAddress(addressID);
            //atHomePage.VerifyHeading("Are you at home?");
            //atHomePage.SelectAtHomeYes();

            //var confirmDetails = personalDetailsPage.SubmitAtHome();
            //confirmDetails.VerifyHeading("Check details");
            //need to submit call
            //Verify text 
            //resubmit
            //Verify text 

        }


        [Test]
        public void ConfirmationscreensPharmacy()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Eye problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "CO12HU");

            questionPage.VerifyQuestion("What is the main problem?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(2)
                .AnswerSuccessiveByOrder(3, 6)
                .Answer(4)
                .Answer<OutcomePage>(1);

            outcomePage.VerifyOutcome("Your answers suggest you should contact a pharmacist within 24 hours");

            //need to insert clicking 'Find a pharmacy link'
            //need to select DoS ID 2000014051


            //personalDetailsPage.VerifyIsPersonalDetailsPage();
            //personalDetailsPage.VerifyNameDisplayed();
            //personalDetailsPage.VerifyNumberDisplayed();
            //personalDetailsPage.VerifyDateOfBirthDisplayed();

            //personalDetailsPage.SelectMe();
            //personalDetailsPage.EnterPatientName("Test1", "Tester1");

            //personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            //personalDetailsPage.EnterPhoneNumber("07793346301");

            //var currentAddressPage = personalDetailsPage.SubmitPersonalDetails();
            //currentAddressPage.VerifyHeading("Where are you right now?");

            //var addressID = "5150786";
            //currentAddressPage.VerifyAddressDisplays(addressID);

            //var atHomePage = currentAddressPage.ClickAddress(addressID);
            //atHomePage.VerifyHeading("Are you at home?");
            //atHomePage.SelectAtHomeYes();

            //var confirmDetails = personalDetailsPage.SubmitAtHome();
            //confirmDetails.VerifyHeading("Check details");
            //need to submit call
            //Verify text 
            //resubmit
            //Verify text 


        }
        [Test]
        public void ConfirmationscreensMidwifery()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female, TestScenerioAgeGroups.Adult, "E173AX");

            questionPage.VerifyQuestion("Is there a chance you're pregnant?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 6)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyOutcome("Your answers suggest you should speak to your midwife within 1 hour");

            //need to insert clicking 'Find a service link'
            //need to select DoS ID 2000006999


            //personalDetailsPage.VerifyIsPersonalDetailsPage();
            //personalDetailsPage.VerifyNameDisplayed();
            //personalDetailsPage.VerifyNumberDisplayed();
            //personalDetailsPage.VerifyDateOfBirthDisplayed();

            //personalDetailsPage.SelectMe();
            //personalDetailsPage.EnterPatientName("Test1", "Tester1");

            //personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            //personalDetailsPage.EnterPhoneNumber("07793346301");

            //var currentAddressPage = personalDetailsPage.SubmitPersonalDetails();
            //currentAddressPage.VerifyHeading("Where are you right now?");

            //var addressID = "55629068";
            //currentAddressPage.VerifyAddressDisplays(addressID);

            //var atHomePage = currentAddressPage.ClickAddress(addressID);
            //atHomePage.VerifyHeading("Are you at home?");
            //atHomePage.SelectAtHomeYes();

            //var confirmDetails = personalDetailsPage.SubmitAtHome();
            //confirmDetails.VerifyHeading("Check details");

            //need to submit call
            //Verify text 
            //resubmit
            //Verify text
        }


    }

 }