using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Configuration;
using OpenQA.Selenium.Support.UI;

namespace NHS111.Web.Functional.Utils
{
    public class LayoutPage
    {
        public static string _baseUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"];

        public readonly IWebDriver Driver;
        internal const string _headerLogoTitle = "Go to the NHS 111 homepage";

        internal const string _headerText = "1 1 1 online";

        [FindsBy(How = How.CssSelector, Using = ".global-header")]
        internal IWebElement Header { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".global-header__logo")]
        internal IWebElement HeaderLogo { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".global-footer")]
        internal IWebElement Footer { get; set; }

        public LayoutPage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(Driver, this);
        }

        public void VerifyLayoutPagePresent()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.IsTrue(HeaderLogo.Displayed);
            Assert.AreEqual(_headerLogoTitle, HeaderLogo.GetAttribute("title"));
            Assert.IsTrue(Footer.Displayed);
        }


        public void VerifyHeaderBannerDisplayed()
        {
            Assert.IsTrue(Header.Displayed);
        }


        public void VerifyHeaderBannerHidden()
        {
            Assert.IsFalse(Header.Displayed);
        }

        public string GetUrlWithoutCredentials()
        {
            if (UrlContainsCredentials())
            {
                return Driver.Url.Remove(_baseUrl.IndexOf("://") + 3,
                    _baseUrl.LastIndexOf("@") - (_baseUrl.IndexOf("://") + 3));
            }
            return _baseUrl;
        }

        public static bool UrlContainsCredentials()
        {
            return _baseUrl.Contains("@");
        }

        public void WaitForElement(IWebElement element)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(drv => element.Displayed);
        }

    }
}
