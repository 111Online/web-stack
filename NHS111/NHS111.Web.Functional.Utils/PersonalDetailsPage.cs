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

        public PersonalDetailsPage SubmitPersonalDetails()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new PersonalDetailsPage(Driver);
        }
        public TelephoneNumberPage SubmitNameAndDoBDetails()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new TelephoneNumberPage(Driver);
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
    }
}