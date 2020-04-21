using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class ReferralExplanationPage : LayoutPage
    {

        [FindsBy(How = How.CssSelector, Using = "button[value='Enter my details']")]
        private IWebElement EnterDetailsButton { get; set; }

        public ReferralExplanationPage(IWebDriver driver) : base(driver) { }

        public PersonalDetailsPage ClickButton()
        {
            EnterDetailsButton.Click();
            return new PersonalDetailsPage(Driver);
        }
    }
}