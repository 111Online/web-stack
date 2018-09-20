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
    using System.Collections.Specialized;
    using System.Web;

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



        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
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
