using System;
using System.Configuration;
using System.Linq;
using System.Web;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class DirectLinking : LayoutPage
    {

        public DirectLinking(IWebDriver driver) : base(driver)
        {
        }

        public static DirectLinking Start(IWebDriver driver)
        {
            return new DirectLinking(driver);
        }

        public DirectLinking Visit(string path)
        {
            Driver.Navigate().GoToUrl(_baseUrl + path);
            if (UrlContainsCredentials())
                Driver.Navigate().GoToUrl(GetUrlWithoutCredentials());
            return this;
        }

        public void VerifyId(string id)
        {
            Assert.IsTrue(Driver.FindElement(By.Id("Id")).GetAttribute("value") == id);
        }

        public void VerifyBookACall(string serviceId)
        {
            Assert.IsTrue(Driver.FindElement(By.Id("SelectedServiceId")).GetAttribute("value") == serviceId);
        }

        public void VerifyServiceDetails()
        {
            Assert.IsTrue(IsDisplayed(Driver.FindElement(By.CssSelector("service-details"))));
        }
    }
}
