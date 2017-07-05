using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.SmokeTest.Utils
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private static string _baseUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"].ToString();

        private const string _headerText = "Getting care with 111 Online";

        [FindsBy(How = How.CssSelector, Using = "h1.discHead")]
        public IWebElement Header { get; set; }

        [FindsBy(How = How.ClassName, Using = "button-get-started")]
        public IWebElement NextButton { get; set; }


        public HomePage(IWebDriver driver) 
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public void Load()
        {
            _driver.Navigate().GoToUrl(_baseUrl);
            _driver.Manage().Window.Maximize();
        }
        public ModuleZeroPage ClickNextButton()
        {
            NextButton.Click();
            return new ModuleZeroPage(_driver);
        }
        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }
    }
}
