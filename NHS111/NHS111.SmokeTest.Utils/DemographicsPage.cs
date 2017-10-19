using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.SmokeTest.Utils
{
    public class DemographicsPage : LayoutPage
    {
        private string _headerText = "Tell us about you, or the person you're asking about";
        private string _ageValidationMessageTooOld = "Please enter a value less than or equal to 200.";
        private string _ageValidationMessageTooYoung = "Sorry, this service is not available for children under 5 years of age, for medical advice please call 111.";

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[for='Male']")]
        private IWebElement MaleButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[for='Female']")]
        private IWebElement FemaleButton { get; set; }

        [FindsBy(How = How.Id, Using = "UserInfo_Demography_Age")]
        private IWebElement AgeInput { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span[for='UserInfo_Demography_Age']")]
        private IWebElement AgeValidationMessageElement { get; set; }

        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NextButton { get; set; }

        public DemographicsPage(IWebDriver driver) : base(driver)
        {
        }

        public void SelectGenderAndAge(string gender, int age)
        {
            SelectGender(gender);
            SetAge(age);
        }

        public void SelectGender(string gender)
        {
            if (gender == DemographicsPage.Male)
                MaleButton.Click();
            else
                FemaleButton.Click();
        }

        public void SetAge(int age)
        {
            AgeInput.SendKeys(age.ToString());
        }

        public void VerifyHeader()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

        public SearchPage NextPage()
        {
            NextButton.Submit();
            return new SearchPage(Driver);
        }

        public void VerifyAgeInputBox(string gender, string age)
        {
            SelectGender(gender);
            AgeInput.SendKeys(age);
            NextButton.Submit();

            var searchPage = new SearchPage(Driver);
            searchPage.Verify();
        }

        public void VerifyTooOldAgeShowsValidation(string gender, int age)
        {
            SelectGender(gender);
            SetAge(age);
            NextButton.Submit();

            Assert.IsTrue(AgeValidationMessageElement.Displayed);
            Assert.AreEqual(AgeValidationMessageElement.Text, _ageValidationMessageTooOld);
        }

        public void VerifyTooYoungAgeShowsValidation(string gender, int age)
        {
            SelectGender(gender);
            SetAge(age);
            NextButton.Submit();

            Assert.IsTrue(AgeValidationMessageElement.Displayed);
            Assert.AreEqual(AgeValidationMessageElement.Text, _ageValidationMessageTooYoung);
        }

        public static string Male = "Male";
        public static string Female = "Female";
    }
}
