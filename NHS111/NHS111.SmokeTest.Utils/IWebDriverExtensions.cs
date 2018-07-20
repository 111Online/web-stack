
namespace NHS111.SmokeTest.Utils {
    using System;
    using System.Linq;
    using OpenQA.Selenium;

    public static class IWebDriverExtensions {
        public static bool ElementExists(this IWebDriver driver, By by) {
            return driver.FindElements(by).Any();
        }


        public static bool IfElementExists(this IWebDriver driver, By by, Action<IWebElement> action) {
            var elements = driver.FindElements(by);
            if (!elements.Any())
                return false;

            action(elements.First());

            return true;
        }

        public static IWebElement TabFrom(this IWebDriver driver, IWebElement element) {
            element.SendKeys(Keys.Tab);
            return driver.SwitchTo().ActiveElement();
        }
    }
}
