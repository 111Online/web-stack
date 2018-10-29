using System;
using System.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.SmokeTest.Utils
{
    using System.Web;

    public class HomePage : LayoutPage
    {
        private static string _baseUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"];

        [FindsBy(How = How.CssSelector, Using = "nav > ul > li:nth-child(1) > a")]
        internal IWebElement TermsLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = "nav > ul > li:nth-child(2) > a")]
        internal IWebElement PrivacyLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = "nav > ul > li:nth-child(3) > a")]
        internal IWebElement CookiesLink { get; set; }

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
            if (UrlContainsCredentials()) {
                Uri uri = new Uri(GetUrlWithoutCredentials());
                uri = uri.AddOrReplaceQuery("utm_medium", mediumQuerystring);

                Driver.Navigate().GoToUrl(uri);
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

        public PrivacyStatementPage ClickPrivacyStatementLink()
        {
            PrivacyLink.Click();

            //link opens in new tab, grab the new tab
            foreach (var winHandle in Driver.WindowHandles)
            {
                Driver.SwitchTo().Window(winHandle);
            }

            return new PrivacyStatementPage(Driver);
        }

        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.IsTrue(TermsLink.Displayed);
            Assert.IsTrue(PrivacyLink.Displayed);
            Assert.IsTrue(CookiesLink.Displayed);

            Assert.AreEqual(_headerText, Header.Text);
        }
    }

    public static class UriExtensions {
        public static Uri AddOrReplaceQuery(this Uri operand, string key, string value) {
            var uriBuilder = new UriBuilder(operand);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[key] = value;
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }
    }
}
