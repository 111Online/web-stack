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
    public class GenderPage
    {
        private readonly IWebDriver _driver;

        private string _headerText = "Tell us about you, or the person you're asking about";

        [FindsBy(How = How.CssSelector, Using = "h1.heading-large")]
        public IWebElement Header { get; set; }


        [FindsBy(How = How.Id, Using = "Male")]
        public IWebElement MaleButton { get; set; }

        [FindsBy(How = How.Id, Using = "Female")]
        public IWebElement FemaleButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "input-age")]
        public IWebElement AgeInput { get; set; }

        [FindsBy(How = How.ClassName, Using = "button-next")]
        public IWebElement NextButton { get; set; }

        public GenderPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public void SelectGenderAndAge(string gender, int age)
        {
            SelectGender(gender);
            SetAge(age);
        }

        public void SelectGender(string gender)
        {
            if (gender == GenderPage.Male)
                MaleButton.Click();
            else
                FemaleButton.Click();
        }

        public void SetAge(int age)
        {
            AgeInput.SendKeys(age.ToString());
        }

        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

        public SearchPage NextPage()
        {
            NextButton.Submit();
            return new SearchPage(_driver);
        }


        public static string Male = "Male";
        public static string Female = "Female";
    }
}
