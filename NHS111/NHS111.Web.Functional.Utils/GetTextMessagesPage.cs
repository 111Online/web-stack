using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class GetTextMessagesPage : LayoutPage
    {
        public GetTextMessagesPage(IWebDriver driver)
            : base(driver) { }

        [FindsBy(How = How.Id, Using = "nextScreen")]
        private IWebElement StartNowButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "nhsuk-heading-xl")]
        public IWebElement HeadingElement { get; set; }

        public QuestionPage NextPage()
        {
            StartNowButton.Click();
            return new QuestionPage(Driver);
        }

        public void VerifyPageContent()
        {
            Assert.AreEqual("Get text messages from the NHS about coronavirus", HeadingElement.Text);
            Assert.AreEqual("Start now", StartNowButton.Text);
        }
    }
}
