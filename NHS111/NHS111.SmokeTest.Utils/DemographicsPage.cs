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

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[for='Male']")]
        private IWebElement MaleButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[for='Female']")]
        private IWebElement FemaleButton { get; set; }

        [FindsBy(How = How.Id, Using = "UserInfo_Demography_Age")]
        private IWebElement AgeInput { get; set; }

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

        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

        public SearchPage NextPage()
        {
            NextButton.Submit();
            return new SearchPage(Driver);
        }


        public static string Male = "Male";
        public static string Female = "Female";
    }
}
