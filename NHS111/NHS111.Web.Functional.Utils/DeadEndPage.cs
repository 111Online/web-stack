using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class DeadEndPage : LayoutPage //currently the DispositionPage<T> markup doesn't match the Dead End page so can't inherit from that
    {

        [FindsBy(How = How.CssSelector, Using = "h2")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.Id, Using = "previousScreen")]
        private IWebElement PreviousAnswer { get; set; }

        public DeadEndPage(IWebDriver driver) : base(driver) { }

        public void VerifyOutcome(string outcomeHeadertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(outcomeHeadertext, Header.Text);
        }

        public QuestionPage ClickPrevious()
        {
            PreviousAnswer.Click();
            return new QuestionPage(Driver);
        }

        public DeadEndPage CompareAndVerify(string uniqueId)
        {
            return base.CompareAndVerify(this, uniqueId);
        }
    }
}