using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Configuration;
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

        public T MakeScreenshot<T>(T page) where T : IScreenshotMaker
        {
            var screenshot = Driver.TakeScreenshot();
            screenshot.SaveAsFile(CreateScreenshotFilename(),ScreenshotImageFormat.Png);
            return page;
        }

        private string CreateScreenshotFilename()
        {
            return $"{CreateScreenshotDir()}{TestContext.CurrentContext.Test.FullName}.png";
        }

        private string CreateScreenshotDir()
        {
            System.IO.Directory.CreateDirectory(_screenshotDir);
            return _screenshotDir;
        }

        public bool CompareScreenshot() 
        {
            _screenshotsEqual = ScreenshotComparer.Compare($"{TestContext.CurrentContext.Test.FullName}.png", _baselineScreenshotsDir, _screenshotDir
            );
            return _screenshotsEqual;

        }

        public T MakeAndCompareScreenshot<T>(T page) where T : IScreenshotMaker
        {
            page = MakeScreenshot(page);
            page.CompareScreenshot();
            return page;
        }

        public T CompareAndVerify<T>(T page) where T : IScreenshotMaker
        {
            var failureAction = (SceenshotComparisonFailureAction)Enum.Parse(typeof(SceenshotComparisonFailureAction), _SceenshotComparisonFailureAction);
            CompareAndVerify(failureAction, page);
            return page;
        }

        
        public T CompareAndVerify<T>(SceenshotComparisonFailureAction action, T page) where T : IScreenshotMaker
        {
            MakeAndCompareScreenshot(page);
            if(action == SceenshotComparisonFailureAction.FailTest && !page.ScreenshotsEqual) Assert.Fail("Screenshot comparison shows not equal to baseline");
            if (action == SceenshotComparisonFailureAction.Warn)
            {
                if(!page.ScreenshotsEqual) Assert.Inconclusive("Screenshot comparison shows not equal to baseline");
            }

            return page;
        }
    }
}
