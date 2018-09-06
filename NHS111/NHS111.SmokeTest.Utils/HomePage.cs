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
    public class HomePage : LayoutPage
    {
        private static string _baseUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"];


        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NextButton { get; set; }
        
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        public void Load()
        {
            Driver.Navigate().GoToUrl(_baseUrl);
            if (UrlContainsCredentials())
            {
                Driver.Navigate().GoToUrl(GetUrlWithoutCredentials());
            }
            Driver.Manage().Window.Maximize();
        }


        public void Load(string mediumQuerystring)
        {
            Driver.Navigate().GoToUrl(_baseUrl);
            if (UrlContainsCredentials())
            {
                if(_baseUrl.Contains("?"))
                    Driver.Navigate().GoToUrl(GetUrlWithoutCredentials() + "&utm_medium=" + mediumQuerystring);
                else
                    Driver.Navigate().GoToUrl(GetUrlWithoutCredentials() + "?utm_medium=" + mediumQuerystring);
            }
            Driver.Manage().Window.Maximize();
        }

        private string GetUrlWithoutCredentials()
        {
            if (UrlContainsCredentials())
            {
                return Driver.Url.Remove(_baseUrl.IndexOf("://") + 3,
                    _baseUrl.LastIndexOf("@") - (_baseUrl.IndexOf("://") + 3));
            }
            return _baseUrl;
        }

        private static bool UrlContainsCredentials()
        {
            return _baseUrl.Contains("@");
        }

        public DemographicsPage ClickNextButton()
        {
            NextButton.Click();
            return new DemographicsPage(Driver);
        }



        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }
    }
}
