using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace NHS111.Web.Functional.Utils
{
    internal class PageLoadAwaiter
    {
        private IWebDriver _driver;

        internal PageLoadAwaiter(IWebDriver driver)
        {
            _driver = driver;
        }

        internal void AwaitNextPage(IWebElement currentHeader, bool expectQuestionPage = true)
        {
            AwaitNextPage(currentHeader, By.CssSelector(".multiple-choice--radio"), 10, expectQuestionPage);
        }

        internal void AwaitNextPage(IWebElement currentHeader, By elementToWaitForBySelector, int timeoutSeconds =10, bool expectQuestionPage = true)
        {
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            try
            {
                if (expectQuestionPage)
                {
                    new WebDriverWait(_driver, timeout).Until(ExpectedConditions.ElementExists(elementToWaitForBySelector));
                }
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail(string.Format("The next page didn't load in the awaited time ({0}s). Current page title is '{1}'.", timeout.Seconds, currentHeader.Text));
            }
            new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
        }

    }
}
