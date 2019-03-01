using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace NHS111.Web.Functional.Utils
{
    public class LayoutPage : IScreenshotMaker
    {
        public static string _baseUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"];
        public static string _baselineScreenshotsDir = ConfigurationManager.AppSettings["BaselineScreenshotsDir"];
        private static string _screenshotDir = $"{TestContext.CurrentContext.WorkDirectory}Screenshots\\";
        private static string _SceenshotComparisonFailureAction = ConfigurationManager.AppSettings["SceenshotComparisonFailureAction"];
        public readonly IWebDriver Driver;
        internal const string _headerLogoTitle = "Go to the NHS 111 homepage";

        internal const string _headerText = "1 1 1 online";

        private bool _screenshotsEqual = false;
        [FindsBy(How = How.CssSelector, Using = ".global-header")]
        internal IWebElement Header { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".global-header__logo")]
        internal IWebElement HeaderLogo { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".global-footer")]
        internal IWebElement Footer { get; set; }

        public bool ScreenshotsEqual => _screenshotsEqual;

        public LayoutPage(IWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(Driver, this);
        }

        public void VerifyLayoutPagePresent()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.IsTrue(HeaderLogo.Displayed);
            Assert.AreEqual(_headerLogoTitle, HeaderLogo.GetAttribute("title"));
            Assert.IsTrue(Footer.Displayed);
        }



        public void VerifyHeaderBannerDisplayed()
        {
            Assert.IsTrue(Header.Displayed);
        }


        public void VerifyHeaderBannerHidden()
        {
            Assert.IsFalse(Header.Displayed);
        }

        public string GetUrlWithoutCredentials()
        {
            if (UrlContainsCredentials())
            {
                return Driver.Url.Remove(_baseUrl.IndexOf("://") + 3,
                    _baseUrl.LastIndexOf("@") - (_baseUrl.IndexOf("://") + 3));
            }

            return _baseUrl;
        }

        public static bool UrlContainsCredentials()
        {
            return _baseUrl.Contains("@");
        }

        public void WaitForElement(IWebElement element)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(drv => element.Displayed);
        }

        public T MakeScreenshot<T>(T page, string uniqueName) where T : IScreenshotMaker
        {
            var screenshot = Driver.TakeEntireScreenshot();
            screenshot.SaveAsFile(CreateScreenshotFilepath(uniqueName),ScreenshotImageFormat.Png);
            return page;
        }

        private string CreateScreenshotFilename(string uniqueName)
        {
            return $"{TestContext.CurrentContext.Test.FullName}-{uniqueName}.png";
        }

        private string CreateScreenshotFilepath(string uniqueName)
        {
            return $"{CreateScreenshotDir()}{CreateScreenshotFilename(uniqueName)}";
        }


        private string CreateScreenshotDir()
        {
            System.IO.Directory.CreateDirectory(_screenshotDir);
            return _screenshotDir;
        }

        public bool CompareScreenshot(string uniqueName)
        {
            _screenshotsEqual = ScreenshotComparer.Compare(CreateScreenshotFilename(uniqueName),
                _baselineScreenshotsDir, _screenshotDir
            );
            return _screenshotsEqual;

        }

        public T MakeAndCompareScreenshot<T>(T page, string uniqueName) where T : IScreenshotMaker
        {
            page = MakeScreenshot(page, uniqueName);
            page.CompareScreenshot(uniqueName);
            return page;
        }

        public T CompareAndVerify<T>(T page, string uniqueName) where T : IScreenshotMaker
        {
            var failureAction = (SceenshotComparisonFailureAction)Enum.Parse(typeof(SceenshotComparisonFailureAction), _SceenshotComparisonFailureAction);
            CompareAndVerify(failureAction, page, uniqueName);
            return page;
        }

        
        public T CompareAndVerify<T>(SceenshotComparisonFailureAction action, T page, string uniqueName) where T : IScreenshotMaker
        {
            MakeAndCompareScreenshot(page, uniqueName);
            if(action == SceenshotComparisonFailureAction.Fail && !page.ScreenshotsEqual) Assert.Fail($"Screenshot comparison shows not equal to baseline at step {uniqueName}");
            if (action == SceenshotComparisonFailureAction.Warn)
            {
                if(!page.ScreenshotsEqual) Assert.Inconclusive($"Screenshot comparison shows not equal to baseline at step {uniqueName}");
            }

            return page;
        }
    }
}
