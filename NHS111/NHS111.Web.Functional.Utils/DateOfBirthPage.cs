﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.Web.Functional.Utils {
    public class DateOfBirthPage: LayoutPage
    {

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h2")]
        private IList<IWebElement> SectionHeadings { get; set; }

        public DateOfBirthPage(IWebDriver driver) : base(driver) { }

        public void VerifyHeading(string headertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(headertext, Header.Text);
        }

        public void VerifyDateOfBirthDisplayed()
        {
            var nameHeading = SectionHeadings[0];
            Assert.IsTrue(nameHeading.Displayed);

            if (Driver.FindElement(By.Id("PatientInformantDetails_Informant")).Text == "Self")
            {
                Assert.AreEqual(nameHeading.Text, "What is your date of birth?");
            } else
            {
                Assert.AreEqual(nameHeading.Text, "What is their date of birth?");
            }
        }

        public void EnterDateOfBirth(string date, string month, string year)
        {
            var thing = By.Id("UserInfo_Day");
            Driver.FindElement(By.Id("UserInfo_Day")).SendKeys(date);
            Driver.FindElement(By.Id("UserInfo_Month")).SendKeys(month);
            Driver.FindElement(By.Id("UserInfo_Year")).SendKeys(year);
        }

        public TelephoneNumberPage SubmitDateOfBirth()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new TelephoneNumberPage(Driver);
        }

    }
}