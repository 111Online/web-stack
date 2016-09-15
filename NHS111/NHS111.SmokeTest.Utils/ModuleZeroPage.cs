using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.SmokeTest.Utils
{
    public class ModuleZeroPage 
    {
        private readonly IWebDriver _driver;
        private static string _baseUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"].ToString();

        private const string _headerText = "Do I need to call 999?";

        [FindsBy(How = How.ClassName, Using = "closeDisclaimer")]
        public IWebElement NoneApplyButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h2.discHead")]
        public IWebElement Header { get; set; }

        public void Load()
        {
            _driver.Navigate().GoToUrl(_baseUrl);
            _driver.Manage().Window.Maximize();
        }

        public ModuleZeroPage(IWebDriver driver) 
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public SearchPage ClickNoneApplyButton()
        {
            NoneApplyButton.Click();
            return new SearchPage(_driver);
        }
        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

    }
}
