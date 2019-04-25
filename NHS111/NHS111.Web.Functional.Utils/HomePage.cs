using System;
using System.Configuration;
using System.Linq;
using System.Web;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class HomePage : LayoutPage, ISubmitPostcodeResult
    {

        [FindsBy(How = How.CssSelector, Using = "nav > ul > li:nth-child(1) > a")]
        internal IWebElement TermsLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = "nav > ul > li:nth-child(2) > a")]
        internal IWebElement PrivacyLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = "nav > ul > li:nth-child(3) > a")]
        internal IWebElement CookiesLink { get; set; }

        [FindsBy(How = How.Id, Using = "CurrentPostcode")]
        private IWebElement PostcodeField { get; set; }

        [FindsByAll]
        [FindsBy(How = How.TagName, Using = "button", Priority = 0)]
        [FindsBy(How = How.ClassName, Using = "button", Priority = 1)] //this is brittle, button needs an id really.
        private IWebElement NextButton { get; set; }
        
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
            //Driver.Manage().Window.Maximize();
            return this;
        }

        public HomePage ClearPostcodeField()
        {
            PostcodeField.Clear();
            return this;
        }


        public ISubmitPostcodeResult ClickNext()
        {
            NextButton.Click();
            var currentUrl = HttpUtility.UrlDecode(Driver.Url.ToLower());
            if (currentUrl.Contains("/location/find") && Driver.Title.Contains("Is it an emergency?"))
                return new ModuleZeroPage(Driver);

            if (currentUrl.Contains("/location/find") && Driver.Title.Contains("Out of area"))
                return new OutOfAreaPage(Driver);

            if (currentUrl.Contains("babylon") || currentUrl.Contains("ask nhs"))
                return new AppPage(Driver);

            if (currentUrl.Contains("111onlinesuffolk"))
                return new Expert24Page(Driver);

            return this;
        }

        public bool ValidationVisible()
        {
            var message = Driver.FindElements(By.CssSelector(".field-validation-error"));
            if (!message.Any())
                throw new AssertionException("Unable to find validation message on page.");
            return message.First().Displayed;
        }

        public HomePage EnterPostcode(string postcode)
        {
            PostcodeField.SendKeys(postcode);
            return this;
        }

        public bool Redirected()
        {
            return Driver.Url.ToLower().Replace(_baseUrl, "") != "";
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
