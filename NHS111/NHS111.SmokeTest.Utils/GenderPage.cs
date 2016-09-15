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

        private string _headerText = "We need a few more details";

        [FindsBy(How = How.CssSelector, Using = ".content-container h2")]
        public IWebElement Header { get; set; }


        [FindsBy(How = How.ClassName, Using = "maleImage")]
        public IWebElement MaleButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "femaleImage")]
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

        public QuestionPage NextPage()
        {
            NextButton.Click();
            return new QuestionPage(_driver);
        }


        public static string Male = "Male";
        public static string Female = "Female";
    }
}
