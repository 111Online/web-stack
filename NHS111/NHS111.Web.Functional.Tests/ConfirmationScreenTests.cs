
namespace NHS111.Web.Functional.Tests
{
    using NUnit.Framework;
    using OpenQA.Selenium;

        {

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
            outcomePage.ClickCantGetAppointment();
            Driver.FindElement(By.XPath("//input[@value = '2000006999']"));

            //var personalDetailsPage = //??
            var personalDetailsPage = outcomePage.UseThisGPService("0");




            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage(telNumber);
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();


            var addressID = "55629068";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(2, "hours", "Advice_CX221185-Adult-Male",
                "Worsening, head injury");

            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            resubmitCallConfirmationPage.VerifyCallConfirmation(2, "hours", "Advice_CX221185-Adult-Male",
                "Worsening, head injury");
        }

        //Scenario 2
        [Test]
        public void ConfirmationScreenPharmacy()
        {
            // string telNumber = GenerateTelephoneNumber();

            // var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Eye or eyelid problems",
            //     TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "CO12HU");

            // questionPage.VerifyQuestion("What is the main problem?");
            // var outcomePage = questionPage
            //     .AnswerSuccessiveByOrder(3, 3)
            //     .Answer(2)
            //     .AnswerSuccessiveByOrder(3, 6)
            //     .Answer(4)
            //     .Answer<OutcomePage>(1);

            // outcomePage.VerifyOutcome("Your answers suggest you should contact a pharmacist within 24 hours");

            //need to insert clicking 'Find a pharmacy link'

            //need to select DoS ID 2000014051


            //personalDetailsPage.VerifyIsPersonalDetailsPage();
            //personalDetailsPage.VerifyNameDisplayed();
            //personalDetailsPage.VerifyNumberDisplayed();
            //personalDetailsPage.VerifyDateOfBirthDisplayed();

            //personalDetailsPage.SelectMe();
            //personalDetailsPage.EnterPatientName("Test1", "Tester1");

            //personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            //personalDetailsPage.EnterPhoneNumber(telNumber);

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

        //Scenario 3
        [Test]
        public void ConfirmationScreenMidwifery()
        {
            string telNumber = GenerateTelephoneNumber();

            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Headache", TestScenerioSex.Female,
                TestScenerioAgeGroups.Adolescent, "E173AX");

            questionPage.VerifyQuestion("Is there a chance you're pregnant?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(1)
                .AnswerSuccessiveByOrder(3, 3)
                .Answer(5)
                .AnswerSuccessiveByOrder(3, 6)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyFindService(FindServiceTypes.Midwife);
            outcomePage.VerifyOutcome("Your answers suggest you should speak to your midwife within 1 hour");
            //need to insert clicking 'Find a service link'
            outcomePage.FindAService();
            //need to select DoS ID 2000006999
            Driver.FindElement(By.XPath("//input[@value = '2000006999']"));

            var personalDetailsPage = outcomePage.UseThisService("1");
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Dx30 first", "Dx30 last");
            personalDetailsPage.EnterDateOfBirth("31", "07", "1980");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage(telNumber);
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "55629068";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(1, "hour", "Advice_CX221056-Adult-Female", "Head injury");

            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            resubmitCallConfirmationPage.VerifyCallConfirmation(1, "hour", "Advice_CX221056-Adult-Female",
                "Head injury");
        }

        //Scenario 4
        [Test]
        public void ConfirmationScreenMidwiferyLabour()
        {
            string telNumber = GenerateTelephoneNumber();

            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Abdominal Pain", TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult, "E173AX");

            questionPage.VerifyQuestion("Do you have any of the following with the tummy pain?");
            var outcomePage = questionPage
                .Answer(4)
                .Answer(3)
                .Answer(5)
                .Answer(3)
                .AnswerSuccessiveByOrder(1, 3)
                .Answer(3)
                .Answer(7)
                .Answer(4)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyFindService(FindServiceTypes.Midwife);
            outcomePage.VerifyOutcome("Your answers suggest you should speak to your midwife or labour ward now");
            //need to insert clicking 'Find a service link'
            outcomePage.FindAService();
            //need to select DoS ID 2000006999
            Driver.FindElement(By.XPath("//input[@value = '2000006999']"));

            var personalDetailsPage = outcomePage.UseThisService("1");
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Dx50 first", "Dx50 last");
            personalDetailsPage.EnterDateOfBirth("31", "07", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage(telNumber);
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "55629068";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(20, "minutes", "Advice_CX221093-Adult-Female",
                "Pregnancy, labour");
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(20, "minutes", "Advice_CX221093-Adult-Female",
                "Pregnancy, labour");
        }

        //Scenario 5
        [Test]
        public void ConfirmationScreenSexualHealthAdultFemale()
        {
            string telNumber = GenerateTelephoneNumber();

            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Sexual or Menstrual Concerns",
                TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult, "E173AX");

            questionPage.VerifyQuestion("Have you been sexually assaulted recently?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(6)
                .Answer(3)
                .Answer(4)
                .Answer(2)
                .Answer(3)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 5)
                .Answer(1)
                .Answer<OutcomePage>(2);

            outcomePage.VerifyFindService(FindServiceTypes.SexualHealthClinic);
            outcomePage.VerifyOutcome("Your answers suggest you should visit a sexual health clinic");
            outcomePage.FindAService();
            Driver.FindElement(By.XPath("//input[@value = '2000006999']"));

            var personalDetailsPage = outcomePage.UseThisService("1");
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Dx31 first", "Dx31 last");
            personalDetailsPage.EnterDateOfBirth("31", "07", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage(telNumber);
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "55629068";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(24, "hours", "Advice_CX221005-Adult-Female",
                "Genital discharge/irritation");
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(24, "hours", "Advice_CX221005-Adult-Female",
                "Genital discharge/irritation");
        }

        //Scenario 6
        [Test]
        public void ConfirmationScreenDentistForAdultMale()
        {
            string telNumber = GenerateTelephoneNumber();

            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult, "LS72BQ");

            questionPage.VerifyQuestion("What is the main problem today?");
            var outcomePage = questionPage
                .Answer(2)
                .Answer(4)
                .AnswerSuccessiveByOrder(1, 2)
                .Answer(3)
                .Answer<OutcomePage>(3);

            // outcomePage.VerifyFindService(FindServiceTypes.UrgentDental);
            outcomePage.VerifyOutcome("See your dentist urgently");
            outcomePage.FindADentalService();
            Driver.FindElement(By.XPath("//input[@value = '2000014914']"));
            var personalDetailsPage = outcomePage.UseThisService("0");
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectSomeoneElse();
            personalDetailsPage.EnterPatientName("Dx18 first", "Dx18 last");
            personalDetailsPage.EnterThirdPartyName("Test Carer", "Test Carer");
            personalDetailsPage.EnterDateOfBirth("01", "01", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage(telNumber);
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are they right now?");

            var addressID = "13865540";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are they at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(2, "hours", "Advice_CX221021-Adult-Male", "Toothache");
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(2, "hours", "Advice_CX221021-Adult-Male", "Toothache");
        }

        //Scenario 7
        [Test]
        public void ConfirmationScreenDentistForAdultFemale()
        {
            string telNumber = GenerateTelephoneNumber();

            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult, "LS72BQ");

            questionPage.VerifyQuestion("What is the main problem today?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .AnswerSuccessiveByOrder(1, 2)
                .Answer(4)
                .Answer<OutcomePage>(2);

            outcomePage.VerifyOutcome(
                "Your answers suggest you need urgent attention for your dental problem within 4 hours");
            outcomePage.FindAService();
            Driver.FindElement(By.XPath("//input[@value = '2000014914']"));
            var personalDetailsPage = outcomePage.UseThisService("0");
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Dx18 first", "Dx18 last");
            personalDetailsPage.EnterDateOfBirth("01", "01", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage(telNumber);
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "13865540";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(4, "hours", "Advice_CX220593-Adult-Female", "Tooth extraction");
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(4, "hours", "Advice_CX220593-Adult-Female", "Tooth extraction");
        }

        //Scenario 8
        [Test]
        public void ConfirmationScreenGynaecology()
        {
            string telNumber = GenerateTelephoneNumber();

            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Something in the vagina",
                TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult, "BB12FD", "Foreign Body, Vaginal");

            questionPage.VerifyQuestion("Is the problem that you can't remove a tampon, condom or cap?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyOutcome("Book a call with a 111 nurse now");
            Driver.FindElement(By.XPath("//input[@value = '2000004969']"));

            var personalDetailsPage = outcomePage.ClickBookCallback();
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Dx32 first", "Dx32 last");
            personalDetailsPage.EnterDateOfBirth("01", "01", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage(telNumber);
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "50939367";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(20, "minutes");
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(20, "minutes");
        }

        //Scenario 9
        [Test]
        public void ConfirmationScreenDermatology()
        {
            string telNumber = GenerateTelephoneNumber();

            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Sunburn", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult, "AL74HL", "Burn, Sun");

            questionPage.VerifyQuestion("Do you feel generally unwell, apart from the sunburn?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 5)
                .Answer<OutcomePage>(2);

            Driver.FindElement(By.XPath("//input[@value = 'Dx02']"));
            outcomePage.VerifyOutcome("Get a phone call from a nurse");
            Driver.FindElement(By.XPath("//input[@value = '2000005832']"));
            outcomePage.VerifyIsCallbackAcceptancePage();

            var personalDetailsPage = outcomePage.AcceptCallback();
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Dx334 first", "Dx334 last");
            personalDetailsPage.EnterDateOfBirth("01", "01", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage(telNumber);
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "51719871";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(30, "Advice_CX221041-Adult-Male", "Sunburn", false);
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(30, "Advice_CX221041-Adult-Male", "Sunburn", true);
        }

        //Scenario 10
        [Test]
        public void ConfirmationScreenSexualHealthAdultMale()
        {
            string telNumber = GenerateTelephoneNumber();
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Sexual Concerns",
                TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult, "Al74HL");

            var outcomePage = questionPage
                .Answer(3)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 3)
                .AnswerSuccessiveByOrder(4, 2)
                .AnswerSuccessiveByOrder(1, 2)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyOutcome("Get a phone call from a nurse");
            Driver.FindElement(By.XPath("//input[@value = '2000005832']"));

            var outcomeRejectionPage = outcomePage.RejectCallback();
            outcomeRejectionPage.VerifyHeader("Go to an emergency treatment centre urgently");

            var personalDetailsPage = outcomeRejectionPage.ClickBookCallback();
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectSomeoneElse();
            personalDetailsPage.EnterPatientName("Dx32 first", "Dx32 last");
            personalDetailsPage.EnterThirdPartyName("Test Carer", "Test Carer");
            personalDetailsPage.EnterDateOfBirth("01", "01", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage(telNumber);
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            var addressID = "51719871";
            currentAddressPage.VerifyAddressDisplays(addressID);
            currentAddressPage.ClickAddress(addressID);
            currentAddressPage.VerifyHeading("Are they at home?");
            personalDetailsPage.SelectAtHomeYes();
            //need to submit call
            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifySexualConcernsCallConfirmation(4, "hours", false);
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = personalDetailsPage.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifySexualConcernsCallConfirmation(4, "hours", true);
        }

        private string GenerateTelephoneNumber()
        {
            Random generator = new Random();
            string prefix = "0777";
            string suffix = generator.Next(0, 999999).ToString("D6");
            return $"{prefix}{suffix}";
        }

        //Scenario 5
        [Test]
        public void ConfirmationScreenSexualHealth()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Sexual or Menstrual Concerns",
                TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult, "E173AX");

            questionPage.VerifyQuestion("Have you been sexually assaulted recently?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(6)
                .Answer(3)
                .Answer(4)
                .Answer(2)
                .Answer(3)
                .Answer(4)
                .AnswerSuccessiveByOrder(3, 5)
                .Answer(1)
                .Answer<OutcomePage>(2);

            outcomePage.VerifyFindService(FindServiceTypes.SexualHealthClinic);
            outcomePage.VerifyOutcome("Your answers suggest you should visit a sexual health clinic");
            outcomePage.FindAService();
            Driver.FindElement(By.XPath("//input[@value = '2000006999']"));

            var personalDetailsPage = outcomePage.UseThisService("1");
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Dx31 first", "Dx31 last");
            personalDetailsPage.EnterDateOfBirth("31", "07", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage("07793346301");
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "55629068";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(24, "hours", "Advice_CX221005-Adult-Female",
                "Genital discharge/irritation");
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(24, "hours", "Advice_CX221005-Adult-Female",
                "Genital discharge/irritation");
        }

        //Scenario 6
        [Test]
        public void ConfirmationScreenDentistForAdultMale()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult, "LS72BQ");

            questionPage.VerifyQuestion("What is the main problem today?");
            var outcomePage = questionPage
                .Answer(2)
                .Answer(4)
                .AnswerSuccessiveByOrder(1, 2)
                .Answer(3)
                .Answer<OutcomePage>(3);

            // outcomePage.VerifyFindService(FindServiceTypes.UrgentDental);
            outcomePage.VerifyOutcome("See your dentist urgently");
            outcomePage.FindADentalService();
            Driver.FindElement(By.XPath("//input[@value = '2000014914']"));
            var personalDetailsPage = outcomePage.UseThisService("0");
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectSomeoneElse();
            personalDetailsPage.EnterPatientName("Dx18 first", "Dx18 last");
            personalDetailsPage.EnterThirdPartyName("Test Carer", "Test Carer");
            personalDetailsPage.EnterDateOfBirth("01", "01", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage("07770728206");
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are they right now?");

            var addressID = "13865540";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are they at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(2, "hours", "Advice_CX221021-Adult-Male", "Toothache");
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(2, "hours", "Advice_CX221021-Adult-Male", "Toothache");
        }

        //Scenario 7
        [Test]
        public void ConfirmationScreenDentistForAdultFemale()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult, "LS72BQ");

            questionPage.VerifyQuestion("What is the main problem today?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .AnswerSuccessiveByOrder(1, 2)
                .Answer(4)
                .Answer<OutcomePage>(2);

            outcomePage.VerifyOutcome(
                "Your answers suggest you need urgent attention for your dental problem within 4 hours");
            outcomePage.FindAService();
            Driver.FindElement(By.XPath("//input[@value = '2000014914']"));
            var personalDetailsPage = outcomePage.UseThisService("0");
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Dx18 first", "Dx18 last");
            personalDetailsPage.EnterDateOfBirth("01", "01", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage("07770728206");
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "13865540";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(4, "hours", "Advice_CX220593-Adult-Female", "Tooth extraction");
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(4, "hours", "Advice_CX220593-Adult-Female", "Tooth extraction");
        }

        //Scenario 8
        [Test]
        public void ConfirmationScreenGynaecology()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Something in the vagina", TestScenerioSex.Female,
                TestScenerioAgeGroups.Adult, "BB12FD", "Foreign Body, Vaginal");

            questionPage.VerifyQuestion("Is the problem that you can't remove a tampon, condom or cap?");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(3)
                .Answer<OutcomePage>(3);

            outcomePage.VerifyOutcome("Book a call with a 111 nurse now");
            Driver.FindElement(By.XPath("//input[@value = '2000004969']"));

            var personalDetailsPage = outcomePage.ClickBookCallback();
            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.SelectMe();
            personalDetailsPage.EnterPatientName("Dx32 first", "Dx32 last");
            personalDetailsPage.EnterDateOfBirth("01", "01", "1971");
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();

            var personalDetailsPhoneNumberPage = personalDetailsPage.SubmitPersonalDetails();
            personalDetailsPhoneNumberPage.EnterPhoneNumberOnSeparatePage("07770728206");
            personalDetailsPhoneNumberPage.VerifyNumberDisplayedOnSeparatePage();

            var currentAddressPage = personalDetailsPhoneNumberPage.SubmitPersonalDetails();
            currentAddressPage.VerifyHeading("Where are you right now?");

            var addressID = "50939367";
            currentAddressPage.VerifyAddressDisplays(addressID);

            var atHomePage = currentAddressPage.ClickAddress(addressID);
            atHomePage.VerifyHeading("Are you at home?");
            atHomePage.SelectAtHomeYes();

            var confirmDetails = personalDetailsPage.SubmitAtHome();
            confirmDetails.VerifyHeading("Check details");
            //need to submit call
            var callConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(20, "minutes");
            //resubmit
            callConfirmationPage.Driver.Navigate().Back();
            var resubmitCallConfirmationPage = confirmDetails.SubmitCall();
            //Verify text 
            callConfirmationPage.VerifyCallConfirmation(20, "minutes");
        }
    }
}
