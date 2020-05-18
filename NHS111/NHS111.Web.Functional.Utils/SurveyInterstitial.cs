using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class SurveyInterstitial : LayoutPage
    {


        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Heading { get; set; }

        [FindsBy(How = How.CssSelector, Using = "main a.button")]
        private IWebElement StartSurveyLink { get; set; }

        public SurveyInterstitial(IWebDriver driver) : base(driver) { }

        public bool VerifyHeading(string text)
        {
            return Heading.Text.Equals(text);
        }

        public bool VerifyUrl(string url)
        {
            return StartSurveyLink.GetAttribute("href").Equals(url);
        }

        public SurveyInterstitial CompareAndVerify(string uniqueId)
        {
            return base.CompareAndVerify(this, uniqueId);
        }
    }
}