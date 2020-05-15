using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.Web.Functional.Utils {
    public class YourNamePage: LayoutPage
    {

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }


        [FindsBy(How = How.CssSelector, Using = "h2")]
        private IList<IWebElement> SectionHeadings { get; set; }

        public YourNamePage(IWebDriver driver) : base(driver) { }

        public void VerifyHeading(string headertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(headertext, Header.Text);
        }


        public void VerifyNameDisplayed()
        {
            var nameHeading = SectionHeadings[0];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "What is your name?");
        }


        public void EnterYourName(string forename, string surname)
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

        public DateOfBirthPage SubmitYourNameDetails()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new DateOfBirthPage(Driver);
        }

        public TheirNamePage SubmitYourNameAsInformant()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new TheirNamePage(Driver);
        }

        

        public void VerifyThirdPartyBannerIsDisplayed()
        {
            Assert.IsTrue(Driver.FindElement(By.Id("confirm-details-third-party")).Displayed);
        }

        public void VerifyThirdPartyBannerNotDisplayed()
        {
            Assert.False(Driver.ElementExists(By.Id("confirm-details-third-party")));
        }

    }
}