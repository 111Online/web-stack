using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;

namespace NHS111.Web.Functional.Utils
{
    public class InlineCareAdvicePage : LayoutPage
    {
        private const string _inlineCareAdviceTitle = "Before you continue, here's some advice for you to follow";

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/form/h1[1]")]
        private IWebElement InlineCareAdviceTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"answers\"]/ul/li[1]")]
        private IWebElement InlineCareAdviceFirstItem { get; set; }

        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NextButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "button--previous")]
        private IWebElement PreviousButton { get; set; }

        private readonly PageLoadAwaiter _pageLoadAwaiter;
        public InlineCareAdvicePage(IWebDriver driver) : base(driver)
        {
            _pageLoadAwaiter = new PageLoadAwaiter(driver);
        }

        public void Verify()
        {
            Assert.IsTrue(InlineCareAdviceTitle.Displayed);
            Assert.AreEqual(_inlineCareAdviceTitle, InlineCareAdviceTitle.Text);
            Assert.IsTrue(InlineCareAdviceFirstItem.Displayed);
            Assert.IsTrue(NextButton.Displayed);
            Assert.IsTrue(PreviousButton.Displayed);
        }

        public T MoveNext<T>()
        {
            NextButton.Click();
            _pageLoadAwaiter.AwaitNextPage(InlineCareAdviceTitle, typeof(T) == typeof(QuestionPage));

            return (T)Activator.CreateInstance(typeof(T), Driver);
        }

        public T MovePrevious<T>()
        {
            PreviousButton.Click();
            _pageLoadAwaiter.AwaitNextPage(InlineCareAdviceTitle, typeof(T) == typeof(QuestionPage));

            return (T)Activator.CreateInstance(typeof(T), Driver);
        }
    }
}
