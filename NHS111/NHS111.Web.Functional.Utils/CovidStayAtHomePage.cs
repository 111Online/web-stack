using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class CovidStayAtHomePage: LayoutPage
    {
        [FindsBy(How = How.CssSelector, Using = "a[data-event-value='gov.uk coronavirus testing page']")]
        public IWebElement GetTestedLink{ get; set; }
        public CovidStayAtHomePage(IWebDriver driver):base(driver)
        {
        }

        public void VerifyGetTestedLink()
        {
            Assert.IsTrue(GetTestedLink.Displayed, "The Get testted link is not visible but it should be");
        }
    }
}