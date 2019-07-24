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

        public void VerifyNumberDisplayed()
        {
            var nameHeading = SectionHeadings[1];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "Date of birth");
        }

        public void VerifyDateOfBirthDisplayed()
        {
            var nameHeading = SectionHeadings[2];
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
            Driver.FindElement(By.Id("HomeAddressSameAsCurrent_Yes")).Click();
        }

        public void SelectAtHomeNo()
        {
            Driver.FindElement(By.Id("HomeAddressSameAsCurrent_No")).Click();
        }

        public PersonalDetailsPage SubmitAtHome()
        {
            Driver.FindElement(By.Id("submitAtHome")).Click();
            return new PersonalDetailsPage(Driver);
        }


        //public OutcomePage SubmitPersonalDetails(string forename, string surname, string telephoneNumber, string dobDay,
        //    string dobMonth, string dobYear)
        //{
        //    Driver.FindElement(By.Id("PatientInformantDetails_Informant_Self")).Click();
        //    var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
        //    var forenameField =
        //        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("PatientInformantDetails_SelfName_Forename")));
        //    forenameField.SendKeys(forename);
        //    Driver.FindElement(By.Id("PatientInformantDetails_SelfName_Surname")).SendKeys(surname);
        //    Driver.FindElement(By.Id("UserInfo_TelephoneNumber")).SendKeys(telephoneNumber);
        //    Driver.FindElement(By.Id("UserInfo_Day")).SendKeys(dobDay);
        //    Driver.FindElement(By.Id("UserInfo_Month")).SendKeys(dobMonth);
        //    Driver.FindElement(By.Id("UserInfo_Year")).SendKeys(dobYear);
        //    Driver.FindElement(By.CssSelector(".button--next.button--secondary.find-address")).Click();
        //    wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
        //    var addressPicker =
        //        wait.Until(ExpectedConditions.ElementIsVisible(
        //            By.Id("AddressInformation_PatientCurrentAddress_SelectedAddressFromPicker")));
        //    var selectElement = new SelectElement(addressPicker);
        //    var address = selectElement.Options[1].Text;
        //    selectElement.SelectByText(address);
        //    Driver.FindElement(By.Id("home-address-dont-know")).Click();
        //    Driver.FindElement(By.Id("submitDetails")).Click();

        //    return new OutcomePage(Driver);
        //}
    }
}