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
            var timeout = TimeSpan.FromSeconds(20);
            try
            {
                if (expectQuestionPage)
                {
                    new WebDriverWait(_driver, timeout).Until(ExpectedConditions.ElementExists(By.CssSelector(".multiple-choice--radio")));
                }
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail(string.Format("The next question page didn't load in the awaited time ({0}s). Current page title is '{1}'.", timeout.Seconds, currentHeader.Text));
            }
            new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
        }
    }
}
