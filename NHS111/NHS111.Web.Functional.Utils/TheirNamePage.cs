using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.Web.Functional.Utils {
    public class TheirNamePage: LayoutPage
    {
        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }


        [FindsBy(How = How.CssSelector, Using = "h2")]
        private IList<IWebElement> SectionHeadings { get; set; }

        public TheirNamePage(IWebDriver driver) : base(driver) { }

        public void VerifyHeading(string headertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(headertext, Header.Text);
        }


        public void VerifyNameDisplayed()
        {
            var nameHeading = SectionHeadings[0];
            Assert.IsTrue(nameHeading.Displayed);
            Assert.IsTrue(nameHeading.Text == "What is their name?");
        }

        public void EnterTheirName(string forename, string surname)
        {
            Driver.FindElement(By.Id("PatientInformantDetails_InformantName_Forename")).SendKeys(forename);
            Driver.FindElement(By.Id("PatientInformantDetails_InformantName_Surname")).SendKeys(surname);
        }

        public DateOfBirthPage SubmitTheirNameDetails()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new DateOfBirthPage(Driver);
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