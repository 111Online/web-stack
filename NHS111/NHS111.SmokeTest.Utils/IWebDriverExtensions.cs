
namespace NHS111.SmokeTest.Utils {
    using System.Linq;
    using OpenQA.Selenium;

    public static class IWebDriverExtensions {
        public static bool ElementExists(this IWebDriver driver, By by) {
            return driver.FindElements(by).Any();
        }
    }
}
