using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class CovidHomePage : LayoutPage
    {
        [FindsBy(How=How.CssSelector, Using = "h1.nhsuk-heading-xl")]
        private IWebElement Title { get; set; }

        [FindsBy(How=How.CssSelector, Using = "button.button--next")]
        private IWebElement StartNowButton { get; set; }

        public CovidHomePage(IWebDriver driver) : base(driver)
        {
        }

        public void VerifyCovidPathway()
        {
            VerifyHiddenField("PathwayNo", "PW1851");
        }

        public LocationPage ClickOnStartNow()
        {
            StartNowButton.Click();
            return new LocationPage(Driver);
        }
    }
}