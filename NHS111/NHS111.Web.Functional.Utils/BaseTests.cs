using System;
using System.Configuration;
using NHS111.Web.Functional.Utils.ScreenShot;
using System.Drawing;
using NHS111.Features;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NHS111.Web.Functional.Utils
{
    public class BaseTests
    {
        public IWebDriver Driver;
        private readonly IHeadlessTestFeature _headlessTestFeature = new HeadlessTestFeature();

        [TestFixtureSetUp]
        public void InitTestFixture()
        {
            // Ideally we could have multiple size screenshots
            // for Visual Regression Test MVP this uses the same width as Andria's Selenium screenshots (1232px)
            var chromeOptions = new ChromeOptions();
            if (_headlessTestFeature.IsEnabled)
            {
                // Build agents should run headless, and this should fix renderer timeout issues
                chromeOptions.AddArgument("--enable-automation"); // https://stackoverflow.com/a/43840128/1689770
                chromeOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--no-sandbox"); //https://stackoverflow.com/a/50725918/1689770
                chromeOptions.AddArgument("--disable-infobars");
                chromeOptions.AddArgument("--disable-dev-shm-usage");
                chromeOptions.AddArgument("--disable-browser-side-navigation"); //https://stackoverflow.com/a/49123152/1689770
                chromeOptions.AddArgument("--disable-gpu"); // Workaround for renderer timeout https://stackoverflow.com/questions/48450594/selenium-timed-out-receiving-message-from-renderer
            }
            chromeOptions.AddArgument("--window-size=1232,1000"); // Ensure all screenshots are same size across build agents
            Driver = new ChromeDriver(chromeOptions);
        }

        [TestFixtureTearDown]
        public void TearDownTestFixture()
        {
            Driver.Quit();
        }

        [TearDown]
        public void TearDownTest()
        {
            if (TestContext.CurrentContext.Result.Status != TestStatus.Failed) return;

            //output the failed screenshot to results screen in Team City
            if(!ScreenShotMaker.CheckScreenShotExists(Driver.GetCurrentImageUniqueId()))
                ScreenShotMaker.MakeScreenShot(Driver.GetCurrentImageUniqueId());
            Console.WriteLine("##teamcity[testMetadata testName='{0}' name='Test screen' type='image' value='{1}']", TestContext.CurrentContext.Test.FullName, ScreenShotMaker.GetScreenShotFilename(Driver.GetCurrentImageUniqueId()));
        }

        public IScreenShotMaker ScreenShotMaker
        {
            get { return new ScreenShotMaker(Driver); }
        }

        public PostcodeProvider Postcodes = new PostcodeProvider();
        protected static readonly string BaseUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"];
    }
}
