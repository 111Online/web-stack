using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.Web.Functional.Utils {
    public class PersonalDetailsPage: LayoutPage
    {

        [FindsBy(How = How.Id, Using = "FindService_CurrentPostcode")]
        private IWebElement PostcodeField { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }


        [FindsBy(How = How.CssSelector, Using = "h2")]
        private IList<IWebElement> SectionHeadings { get; set; }

        public PersonalDetailsPage(IWebDriver driver) : base(driver) { }

        public void VerifyHeading(string headertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(headertext, Header.Text);
        }

        public void VerifyIsPersonalDetailsPage()
        {
            VerifyHeading("Enter details");
        }



        public void VerifyNameDisplayed()
        {
            var nameHeading = SectionHeadings[0];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "Who needs help?");
        }

        public void VerifyDateOfBirthDisplayed()
        {
            var nameHeading = SectionHeadings[1];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "Date of birth");
        }

        public void VerifyNumberDisplayed()
        {
            var nameHeading = SectionHeadings[2];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "What number should we call?");
        }

        public void VerifyNumberDisplayedOnSeparatePage()
        {
            var nameHeading = SectionHeadings[0];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "What number should we call?");
        }

        public void SelectMe()
        {
            Driver.FindElement(By.Id("PatientInformantDetails_Informant_Self")).Click();
        }

        public void SelectSomeoneElse()
        {
            Driver.FindElement(By.Id("PatientInformantDetails_Informant_ThirdParty")).Click();
        }

        public void EnterPatientName(string forename, string surname)
        {
            // For first party
            if (Driver.FindElement(By.Id("PatientInformantDetails_SelfName_Forename")).Displayed)
            {
                Driver.FindElement(By.Id("PatientInformantDetails_SelfName_Forename")).SendKeys(forename);
                Driver.FindElement(By.Id("PatientInformantDetails_SelfName_Surname")).SendKeys(surname);
            }

            // For third party
            if (Driver.FindElement(By.Id("PatientInformantDetails_PatientName_Forename")).Displayed)
            {
                Driver.FindElement(By.Id("PatientInformantDetails_PatientName_Forename")).SendKeys(forename);
                Driver.FindElement(By.Id("PatientInformantDetails_PatientName_Surname")).SendKeys(surname);
            }
        }

        public void EnterThirdPartyName(string forename, string surname)
        {
            Driver.FindElement(By.Id("PatientInformantDetails_InformantName_Forename")).SendKeys(forename);
            Driver.FindElement(By.Id("PatientInformantDetails_InformantName_Surname")).SendKeys(surname);
        }

        public void EnterDateOfBirth(string date, string month, string year)
        {
            Driver.FindElement(By.Id("UserInfo_Day")).SendKeys(date);
            Driver.FindElement(By.Id("UserInfo_Month")).SendKeys(month);
            Driver.FindElement(By.Id("UserInfo_Year")).SendKeys(year);
        }

        public void EnterPhoneNumber(string phone)
        {
            Driver.FindElement(By.Id("UserInfo_TelephoneNumber")).SendKeys(phone);
        }

        public void EnterPhoneNumberOnSeparatePage(string phone)
        {
            Driver.FindElement(By.Id("TelephoneNumber")).SendKeys(phone);
        }

        public PersonalDetailsPage SubmitPersonalDetails()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new PersonalDetailsPage(Driver);
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

        public void VerifyCallConfirmation(int duration, string unitOfTime, string adviceId, string expectedId)
        {
            VerifyHeading("Your call is confirmed");
            var firstSectionHeading = Driver.FindElement(By.ClassName("local-header__intro")).Text;
            string expectedConfirmationMessage = $"If you haven't had a call within {duration} {unitOfTime}, please call 111";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                string.Format("Possible unexpected header. Expected header text of '{0}' but was '{1}'.",
                    expectedConfirmationMessage, firstSectionHeading));

            var secondSectionHeading = Driver.FindElement(By.ClassName("care-advice"));
            Assert.IsTrue(secondSectionHeading.Text.Contains("What you can do in the meantime"));

            var secondSectionId = secondSectionHeading.FindElement(By.Id(adviceId)).Text;
            Assert.AreEqual(expectedId,secondSectionId,
                string.Format("Possible unexpected Id. Expected Id text of '{0}' but was '{1}'.",
                    expectedId, secondSectionId));
        }
        public void VerifyCallConfirmation(int duration, string unitOfTime)
        {
            string heading = "Your call is confirmed";
            VerifyHeading(heading);
            var firstSectionHeading = Driver.FindElement(By.ClassName("local-header__intro")).Text;
            string expectedConfirmationMessage = $"If you haven't had a call within {duration} {unitOfTime}, please call 111";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                string.Format("Possible unexpected header. Expected header text of '{0}' but was '{1}'.",
                    expectedConfirmationMessage, firstSectionHeading));
        }

        public void VerifyCallConfirmation(int duration, string unitOfTime, Boolean hasBeenResubmitted)
        {
            string heading = hasBeenResubmitted ? "Your call has already been booked" : $"You should get a call within {duration} {unitOfTime}.";
            VerifyHeading(heading);
            var firstSectionHeading = hasBeenResubmitted ? Driver.FindElement(By.CssSelector("div.callout.callout--warning > p")).Text :
                Driver.FindElement(By.ClassName("local-header__intro")).Text;
            string expectedConfirmationMessage = hasBeenResubmitted ?
                    $"If you haven't had a call within {duration} {unitOfTime}, please call 111" :
                "If your symptoms get worse go to the nearest A&E (accident and emergency department)."; 

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                string.Format("Possible unexpected header. Expected header text of '{0}' but was '{1}'.",
                    expectedConfirmationMessage, firstSectionHeading));
        }

        public void VerifyCallConfirmation(int duration, string adviceId, string expectedId, Boolean hasBeenResubmitted)
        { 
            string heading = hasBeenResubmitted ? "Your call has already been booked" : $"You should get a call within {duration} minutes.";
            VerifyHeading(heading);
            var firstSectionHeading = hasBeenResubmitted ? Driver.FindElement(By.CssSelector("div.callout.callout--warning > p")).Text : 
                Driver.FindElement(By.ClassName("local-header__intro")).Text;
            string expectedConfirmationMessage = hasBeenResubmitted ? 
                $"If you did not get a call within {duration} minutes, or you've got worse, go to the nearest A&E (accident and emergency department)." : 
                "If your symptoms get worse go to the nearest A&E (accident and emergency department).";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                string.Format("Possible unexpected header. Expected header text of '{0}' but was '{1}'.",
                    expectedConfirmationMessage, firstSectionHeading));

            var secondSectionHeading = Driver.FindElement(By.ClassName("care-advice"));
            Assert.IsTrue(secondSectionHeading.Text.Contains("What you can do in the meantime"));

            var secondSectionId = secondSectionHeading.FindElement(By.Id(adviceId)).Text;
            Assert.AreEqual(expectedId, secondSectionId,
                string.Format("Possible unexpected Id. Expected Id text of '{0}' but was '{1}'.",
                    expectedId, secondSectionId));
        }

        public void VerifySexualConcernsCallConfirmation(int duration, string unitOfTime, Boolean hasBeenResubmitted)
        {
            string heading = hasBeenResubmitted ? "Your call has already been booked" : $"You should get a call within {duration} {unitOfTime}.";
            VerifyHeading(heading);
            var firstSectionHeading = hasBeenResubmitted ? Driver.FindElement(By.CssSelector("div.callout.callout--warning > p")).Text :
                Driver.FindElement(By.ClassName("local-header__intro")).Text;
            string expectedConfirmationMessage = hasBeenResubmitted ?
                $"If you did not get a call within {duration} {unitOfTime}, or you've got worse, go to the nearest A&E (accident and emergency department)." :
                "If your symptoms get worse go to the nearest A&E (accident and emergency department).";

            Assert.AreEqual(expectedConfirmationMessage, firstSectionHeading,
                string.Format("Possible unexpected header. Expected header text of '{0}' but was '{1}'.",
                    expectedConfirmationMessage, firstSectionHeading));
        }
    }
}
