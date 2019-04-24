using System;
using System.Configuration;
using System.Linq;
using System.Web;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class LocationPage : LayoutPage, ISubmitPostcodeResult
    {

        [FindsBy(How = How.Id, Using = "CurrentPostcode")]
        private IWebElement PostcodeField { get; set; }
        
        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NextButton { get; set; }

        public LocationPage(IWebDriver driver) : base(driver)
        {
        }

        public LocationPage ClearPostcodeField()
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

            return this;
        }

        public bool ValidationVisible()
        {
            var message = Driver.FindElements(By.CssSelector(".field-validation-error"));
            if (!message.Any())
                throw new AssertionException("Unable to find validation message on page.");
            return message.First().Displayed;
        }

        public LocationPage EnterPostcode(string postcode)
        {
            PostcodeField.SendKeys(postcode);
            return this;
        }

        public bool Redirected()
        {
            return Driver.Url.ToLower().Replace(_baseUrl, "") != "";
        }
    }
}
