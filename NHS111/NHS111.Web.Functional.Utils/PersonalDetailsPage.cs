using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace NHS111.Web.Functional.Utils
{
    public class PersonalDetailsPage : LayoutPage
    {
        private const string PatientForename = "Forename";
        private const string PatientSurname = "Surname";

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }


        [FindsBy(How = How.CssSelector, Using = "h2")]
        private IList<IWebElement> SectionHeadings { get; set; }

        public PersonalDetailsPage(IWebDriver driver) : base(driver) { }

        //[Ignore]
        public void VerifyHeading(string headertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(headertext, Header.Text);
        }

        public void VerifyNameDisplayed()
        {
            var forename = Driver.FindElement(By.Id(PatientForename));
            var surname = Driver.FindElement(By.Id(PatientSurname));
            Assert.IsTrue(forename.Displayed);
            Assert.IsTrue(surname.Displayed);
            Assert.IsFalse(string.IsNullOrEmpty(forename.GetAttribute("value")));
            Assert.IsFalse(string.IsNullOrEmpty(surname.GetAttribute("value")));
        }

        public void VerifyConfirmNameDisplayed(string Forename, string Surname)
        {
            var name = Driver.FindElement(By.Id("content_name"));
            Assert.AreEqual($"{Forename} {Surname}",name.Text);
        }

        public void VerifyDateOfBirthDisplayed()
        {
            var expectedDateOfBirthSectionHeading = "What is your date of birth?";
            var datOfBirthSectionHeading = SectionHeadings[0];
            Assert.IsTrue(datOfBirthSectionHeading.Displayed);
            Assert.IsTrue(datOfBirthSectionHeading.Text == expectedDateOfBirthSectionHeading);
        }

        public void VerifyNumberDisplayed(int index = 2)
        {
            var nameHeading = SectionHeadings[index];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "What number should we call?");
        }

        public void VerifyNumberDisplayedOnSeparatePage()
        {
            var nameHeading = SectionHeadings[0];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "What number should we call?");
        }

        public void VerifyWhoNeedsHelpDisplayed()
        {
            var nameHeading = SectionHeadings[0];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "Who needs help?");
        }

        public void SelectMe()
        {
            Driver.FindElement(By.Id("Informant_Self")).Click();
        }

        public void SelectSomeoneElse()
        {
            Driver.FindElement(By.Id("Informant_ThirdParty")).Click();
        }

        public void EnterForenameAndSurname(string forename, string surname)
        {
            // For first party
            if (Driver.FindElement(By.Id(PatientForename)).Displayed)
            {
                Driver.FindElement(By.Id(PatientForename)).SendKeys(forename);
                Driver.FindElement(By.Id(PatientSurname)).SendKeys(surname);
            }
        }

        public void EnterThirdPartyName(string forename, string surname)
        {
            Driver.FindElement(By.Id("Forename")).SendKeys(forename);
            Driver.FindElement(By.Id("Surname")).SendKeys(surname);
        }

        public void EnterPhoneNumber(string phone)
        {
            Driver.FindElement(By.Id("TelephoneNumber")).SendKeys(phone);
        }

        public void EnterPhoneNumberOnSeparatePage(string phone)
        {
            Driver.FindElement(By.Id("TelephoneNumber")).SendKeys(phone);
        }

        public void EnterDateOfBirth(string date, string month, string year)
        {
            Driver.FindElement(By.Id("Day")).SendKeys(date);
            Driver.FindElement(By.Id("Month")).SendKeys(month);
            Driver.FindElement(By.Id("Year")).SendKeys(year);
        }

        public void VerifyDateOfBirthByInformantDisplayed()
        {
            string expectedDateOfBirthSectionHeading = "What is their date of birth?";
            var datOfBirthSectionHeading = SectionHeadings[0];
            Assert.IsTrue(datOfBirthSectionHeading.Displayed);
            Assert.IsTrue(datOfBirthSectionHeading.Text == expectedDateOfBirthSectionHeading);
        }

        public PersonalDetailsPage SubmitPersonalDetails()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new PersonalDetailsPage(Driver);
        }

        public YourNamePage SubmitInformantDetails()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new YourNamePage(Driver);
        }

        public DateOfBirthPage SubmitNameDetails()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new DateOfBirthPage(Driver);
        }

        public void VerifyAddressDisplays(string id)
        {
            Assert.IsTrue(Driver.FindElement(By.Id(id)).Displayed);
        }

        public PersonalDetailsPage ClickAddress(string id)
        {
            Driver.FindElement(By.Id(id)).Click();
            return new PersonalDetailsPage(Driver);
        }

        public PersonalDetailsPage ClickAddressNotListed()
        {
            Driver.FindElement(By.Id("AddressNotListed")).Click();
            return new PersonalDetailsPage(Driver);
        }

        public void SelectAtHomeYes()
        {
            Driver.FindElement(By.Id("home-address-yes")).Click();
        }

        public void SelectAtHomeNo()
        {
            Driver.FindElement(By.Id("home-address-no")).Click();
        }

        public PersonalDetailsPage SubmitAtHome()
        {
            Driver.FindElement(By.Id("submitHomeAddress")).Click();
            return new PersonalDetailsPage(Driver);
        }

        public void VerifyThirdPartyBannerIsDisplayed()
        {
            Assert.IsTrue(Driver.FindElement(By.Id("confirm-details-third-party")).Displayed);
        }

        public void VerifyThirdPartyBannerNotDisplayed()
        {
            Assert.False(Driver.ElementExists(By.Id("confirm-details-third-party")));
        }
        
        public void VerifyIsPersonalDetailsPage()
        {
            VerifyHeading("Enter details");
        }
        public void VerifyInformantNameIsEmpty()
        {
            var forename = Driver.FindElement(By.Id("Informant_Forename")).GetAttribute("value");
            var surname = Driver.FindElement(By.Id("Informant_Surname")).GetAttribute("value");
            Assert.IsTrue(string.IsNullOrEmpty(forename));
            Assert.IsTrue(string.IsNullOrEmpty(surname));
        }

        public void TypeHomePostcode(string postcode)
        {
            Driver.FindElement(By.Id("AddressInformation_ChangePostcode_Postcode")).SendKeys(postcode);
        }

        public PersonalDetailsPage SubmitHomePostcode()
        {
            Driver.FindElement(By.Id("changeHomeAddressPostcode")).Click();
            return new PersonalDetailsPage(Driver);
        }

        public PersonalDetailsPage SubmitCall()
        {
            Driver.FindElement(By.Id("book-call")).Click();
            return new PersonalDetailsPage(Driver);
        }

        public PersonalDetailsPage EnterDetailsForPharmacyContact()
        {
            Driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            return new PersonalDetailsPage(Driver);
        }

        public void VerifyCallConfirmation(int duration, string unitOfTime, string adviceId, string expectedId)
        {
            VerifyHeading("Your call is confirmed");
            var firstSectionHeading = Driver.FindElement(By.ClassName("local-header__intro")).Text;
            var expectedConfirmationMessage = $"If you haven't had a call within {duration} {unitOfTime}, please call 111";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                $"Possible unexpected header. Expected header text of '{expectedConfirmationMessage}' but was '{firstSectionHeading}'.");

            var secondSectionHeading = Driver.FindElement(By.ClassName("care-advice"));
            Assert.IsTrue(secondSectionHeading.Text.Contains("What you can do in the meantime"));

            var secondSectionId = secondSectionHeading.FindElement(By.Id(adviceId)).Text;
            Assert.AreEqual(expectedId, secondSectionId,
                $"Possible unexpected Id. Expected Id text of '{expectedId}' but was '{secondSectionId}'.");
        }

        public void VerifyCallConfirmation(int duration, string unitOfTime)
        {
            var heading = "Your call is confirmed";
            VerifyHeading(heading);
            var firstSectionHeading = Driver.FindElement(By.ClassName("local-header__intro")).Text;
            var expectedConfirmationMessage = $"If you haven't had a call within {duration} {unitOfTime}, please call 111";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                $"Possible unexpected header. Expected header text of '{expectedConfirmationMessage}' but was '{firstSectionHeading}'.");
        }

        public void VerifyCallConfirmation(int duration, string adviceId, string expectedId, bool hasBeenResubmitted = false)
        {
            var heading = hasBeenResubmitted ? "Your call has already been booked" : $"You should get a call within {duration} minutes.";
            VerifyHeading(heading);
            var firstSectionHeading = hasBeenResubmitted ? Driver.FindElement(By.CssSelector("div.callout.callout--warning > p")).Text :
                Driver.FindElement(By.ClassName("local-header__intro")).Text;
            var expectedConfirmationMessage = hasBeenResubmitted ?
                $"If you did not get a call within {duration} minutes, or you've got worse, go to the nearest A&E (accident and emergency department)." :
                "If your symptoms get worse go to the nearest A&E (accident and emergency department).";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                $"Possible unexpected header. Expected header text of '{expectedConfirmationMessage}' but was '{firstSectionHeading}'.");

            var secondSectionHeading = Driver.FindElement(By.ClassName("care-advice"));
            Assert.IsTrue(secondSectionHeading.Text.Contains("What you can do in the meantime"));

            var secondSectionId = secondSectionHeading.FindElement(By.Id(adviceId)).Text;
            Assert.AreEqual(expectedId, secondSectionId,
                $"Possible unexpected Id. Expected Id text of '{expectedId}' but was '{secondSectionId}'.");
        }

        public void VerifySexualConcernsCallConfirmation(int duration, string unitOfTime, bool hasBeenResubmitted = false)
        {
            var heading = hasBeenResubmitted ? "Your call has already been booked" : $"You should get a call within {duration} {unitOfTime}.";
            VerifyHeading(heading);

            var firstSectionHeading = hasBeenResubmitted ?
                Driver.FindElement(By.CssSelector("div.callout.callout--warning > p")).Text :
                Driver.FindElement(By.ClassName("local-header__intro")).Text;

            var expectedConfirmationMessage = hasBeenResubmitted ?
                $"If you did not get a call within {duration} {unitOfTime}, or you've got worse, go to the nearest A&E (accident and emergency department)." :
                "If your symptoms get worse go to the nearest A&E (accident and emergency department).";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                $"Possible unexpected header. Expected header text of '{expectedConfirmationMessage}' but was '{firstSectionHeading}'.");
        }

        public void VerifyEmergencyServicesCallConfirmation(int duration, string unitOfTime, bool hasBeenResubmitted = false)
        {
            var heading = hasBeenResubmitted ? "Your call has already been booked" : $"You should get a call within {duration} {unitOfTime}.";
            VerifyHeading(heading);

            var firstSectionHeading = hasBeenResubmitted ?
                Driver.FindElement(By.CssSelector("div.callout.callout--warning > p")).Text :
                Driver.FindElement(By.ClassName("local-header__intro")).Text;

            var expectedConfirmationMessage = hasBeenResubmitted ?
                "If you did not get a call within 30 minutes, or you've got worse, call 999" :
                "Emergency services might be sent to check you're ok if we can't speak to you in that time. If your symptoms get worse call 999.";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                $"Possible unexpected header. Expected header text of '{expectedConfirmationMessage}' but was '{firstSectionHeading}'.");
        }

        public void VerifyMentalHealthEDCallConfirmation(int duration, string unitOfTime, bool hasBeenResubmitted = false)
        {
            var heading = hasBeenResubmitted ? "Your call has already been booked" : $"You should get a call within {duration} {unitOfTime}.";
            VerifyHeading(heading);

            var firstSectionHeading = hasBeenResubmitted ?
                Driver.FindElement(By.CssSelector("div.callout.callout--warning > p")).Text :
                Driver.FindElement(By.CssSelector("div.callout.callout--attention > p")).Text;

            var expectedConfirmationMessage = hasBeenResubmitted ?
                $"If you did not get a call within {duration} {unitOfTime}, or you've got worse, go to the nearest A&E (accident and emergency department)." :
                "If there are any new symptoms, or if the condition gets worse, call 111 for advice.";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                $"Possible unexpected header. Expected header text of '{expectedConfirmationMessage}' but was '{firstSectionHeading}'.");
        }

        public void VerifyEmergencyPrescriptionConfirmation()
        {
            var heading = "Call the pharmacy before you go";
            VerifyHeading(heading);

            var firstSectionHeading = Driver.FindElement(By.CssSelector("div.callout.callout--attention > p")).Text;

            var expectedConfirmationMessage = "Call the pharmacy on 0113 2685602 and give them this reference number:";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                $"Possible unexpected header. Expected header text of '{expectedConfirmationMessage}' but was '{firstSectionHeading}'.");
        }
    }
}
