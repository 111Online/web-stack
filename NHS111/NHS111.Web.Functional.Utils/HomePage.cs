using System;
using System.Configuration;
using System.Linq;
using System.Web;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class HomePage : LayoutPage
    {

        [FindsBy(How = How.CssSelector, Using = "nav > ul > li:nth-child(1) > a")]
        internal IWebElement TermsLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = "nav > ul > li:nth-child(2) > a")]
        internal IWebElement PrivacyLink { get; set; }

        [FindsBy(How = How.Id, Using = "cookie-link")]
        internal IWebElement CookiesLink { get; set; }

        [FindsBy(How = How.Id, Using = "CurrentPostcode")]
        private IWebElement PostcodeField { get; set; }
        
        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NextButton { get; set; }
        
        [FindsBy(How = How.CssSelector, Using = "a[href='/emergency-prescription']")]
        private IWebElement EPDeeplink { get; set; }

        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        public static HomePage Start(IWebDriver driver)
        {
            return new HomePage(driver);
        }

        public HomePage Visit()
        {
            Driver.Navigate().GoToUrl(_baseUrl);
            if (UrlContainsCredentials())
                Driver.Navigate().GoToUrl(GetUrlWithoutCredentials());
            return this;
        }

        public HomePage Visit(string mediumQuerystring)
        {
            var baseUri = new Uri(_baseUrl);
            baseUri = baseUri.AddOrReplaceQuery("utm_medium", mediumQuerystring);
            Driver.Navigate().GoToUrl(baseUri);
            if (UrlContainsCredentials())
            {
                var uri = new Uri(GetUrlWithoutCredentials());
                Driver.Navigate().GoToUrl(uri);
            }
            return this;
        }

        public LocationPage ClickEPDeeplink()
        {
            EPDeeplink.Click();
            return new LocationPage(Driver);
        }

        public LocationPage ClickStart()
        {
            NextButton.Submit();
            return new LocationPage(Driver);
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

        public CookiesStatementPage ClickCookiesStatementLink()
        {
            CookiesLink.Click();

            //link opens in new tab, grab the new tab
            foreach (var winHandle in Driver.WindowHandles)
            {
                Driver.SwitchTo().Window(winHandle);
            }

            return new CookiesStatementPage(Driver);
        }

        public TermsAndConditionsPage ClickTermsLink()
        {
            TermsLink.Click();

            //link opens in new tab, grab the new tab
            foreach (var winHandle in Driver.WindowHandles)
            {
                Driver.SwitchTo().Window(winHandle);
            }

            return new TermsAndConditionsPage(Driver);
        }

        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.IsTrue(TermsLink.Displayed);
            Assert.IsTrue(PrivacyLink.Displayed);
            Assert.IsTrue(CookiesLink.Displayed);

            Assert.AreEqual(_headerText, Header.Text);
        }

        
        public HomePage CompareAndVerify(string uniqueId)
        {
            return base.CompareAndVerify(this, uniqueId);
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
