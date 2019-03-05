using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Configuration;
using System.Diagnostics;
using System.IO;
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
        private static string _screenshotDir = TestContext.CurrentContext.WorkDirectory + "Screenshots\\";
        private static string _screenshotUncomparedDir = _screenshotDir + "uncompared\\";
        private static string _ScreenshotComparisonFailureAction = ConfigurationManager.AppSettings["ScreenshotComparisonFailureAction"];
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

        public bool GetScreenshotsEqual()
        {
            return _screenshotsEqual;
        }

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

        public T MakeScreenshot<T>(T page, string uniqueName, bool uncompared = false ) where T : IScreenshotMaker
        {
            var screenshot = Driver.TakeEntireScreenshot();
            if (uncompared)
            {
                screenshot.SaveAsFile(CreateUncomparedScreenshotFilepath(uniqueName), ScreenshotImageFormat.Png);
            }
            else
            {
                screenshot.SaveAsFile(CreateScreenshotFilepath(uniqueName), ScreenshotImageFormat.Png);
            }
            return page;
        }

        private string CreateScreenshotFilename(string uniqueName)
        {
            return TestContext.CurrentContext.Test.FullName + "-" + uniqueName + ".png";
        }
        
        private string CreateUncomparedScreenshotFilepath(string uniqueName)
        {
            return CreateUncomparedScreenshotDir() + CreateScreenshotFilename(uniqueName);
        }


        private string CreateUncomparedScreenshotDir()
        {
            System.IO.Directory.CreateDirectory(_screenshotUncomparedDir);
            return _screenshotUncomparedDir;
        }

        private string CreateScreenshotFilepath(string uniqueName)
        {
            return CreateScreenshotDir() + CreateScreenshotFilename(uniqueName);
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
            var failureAction = (ScreenshotComparisonFailureAction)Enum.Parse(typeof(ScreenshotComparisonFailureAction), _ScreenshotComparisonFailureAction);
            CompareAndVerify(failureAction, page, uniqueName);
            return page;
        }

        public bool CheckBaselineExists<T>(T page, string uniqueName) where T : IScreenshotMaker
        {
            return File.Exists(_baselineScreenshotsDir + CreateScreenshotFilename(uniqueName));
        }
       
        public T CompareAndVerify<T>(ScreenshotComparisonFailureAction action, T page, string uniqueName) where T : IScreenshotMaker
        {
            if (!CheckBaselineExists(page, uniqueName))
            {
                MakeScreenshot(page, uniqueName, true);
                Assert.Inconclusive("Screenshot comparison baseline missing at step " + uniqueName);
                return page;
            }

            MakeAndCompareScreenshot(page, uniqueName);
            Console.WriteLine("##teamcity[testMetadata testName='{0}' name='setUp time' type='number' value='434.5']", TestContext.CurrentContext.Test.FullName);
            Console.WriteLine("##teamcity[testMetadata testName='{0}' name='some key' value='some value']", TestContext.CurrentContext.Test.FullName);
            Console.WriteLine("##teamcity[testMetadata testName='{0}' type='artifact' value='{1}']", TestContext.CurrentContext.Test.FullName, CreateUncomparedScreenshotFilepath(uniqueName));
            Console.WriteLine("##teamcity[testMetadata testName='{0}' type='image' value='Screenshots/uncompared/{1}']", TestContext.CurrentContext.Test.FullName, CreateScreenshotFilename(uniqueName));
            if (action == ScreenshotComparisonFailureAction.Fail && !page.GetScreenshotsEqual()) Assert.Fail("Screenshot comparison shows not equal to baseline at step " + uniqueName);
            if (action == ScreenshotComparisonFailureAction.Warn)
            {
                if(!page.GetScreenshotsEqual()) Assert.Inconclusive("Screenshot comparison shows not equal to baseline at step " + uniqueName);
            }

            Assert.Fail("REMOVE ONCE DONE Passed but I'm setting it inconclusinve anyway to test meta data");

            return page;
        }
    }
}
