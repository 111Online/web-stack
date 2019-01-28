using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class FeedbackSection : LayoutPage
    {
        
        [FindsBy(How = How.ClassName, Using = "feedback")]
        internal IWebElement Feedback { get; set; }
        
        [FindsBy(How = How.ClassName, Using = "feedback__submit")]
        private IWebElement FeedbackSubmitButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "feedback__input")]
        private IWebElement FeedbackTextarea { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".feedback summary")]
        private IWebElement FeedbackSummary { get; set; }

        public FeedbackSection(IWebDriver driver) : base(driver)
        {
            OpenDetails();
        }

        public void ClickSubmitButton()
        {
            FeedbackSubmitButton.Submit();
        }
        
        public void VerifyFeedbackDisplayed()
        {
            Assert.IsTrue(Feedback.Displayed);
        }

        public void OpenDetails()
        {
            FeedbackSummary.Click();
        }

        public void VerifyButtonDisabled()
        {
            Assert.IsFalse(FeedbackSubmitButton.Enabled);
        }
        public void TypeInTextarea(string text)
        {
            FeedbackTextarea.SendKeys(text);
        }
    }
}
