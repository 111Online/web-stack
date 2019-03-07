using System;
using System.Configuration;
using NHS111.Web.Functional.Utils.ScreenShot;
using System.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NHS111.Web.Functional.Utils
{
    public class BaseTests
    {
        public IWebDriver Driver;

        [TestFixtureSetUp]
        public void InitTestFixture()
        {
            Driver = new ChromeDriver();
            // Ideally we could have multiple size screenshots
            // for Visual Regression Test MVP this uses the same width as Andria's Selenium screenshots (1232px)
            Driver.Manage().Window.Size = new Size(1232, 1000);
        }

        [TestFixtureTearDown]
        public void TearDownTestFixture()
        {
            try
            {
                Driver.Quit();
                Driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        [TearDown]
        public void TearDownTest()
        {
            if (TestContext.CurrentContext.Result.Status == TestStatus.Failed)
            {
                //output the failed screenshot to results screen in Team City
                if(!ScreenShotMaker.CheckScreenShotExists(Driver.GetCurrentImageUniqueId()))
                    ScreenShotMaker.MakeScreenShot(Driver.GetCurrentImageUniqueId());
                Console.WriteLine("##teamcity[testMetadata testName='{0}' name='Test screen' type='image' value='{1}']", TestContext.CurrentContext.Test.FullName, ScreenShotMaker.GetScreenShotFilename(Driver.GetCurrentImageUniqueId()));
            }
        }

        public IScreenShotMaker ScreenShotMaker
        {
            get { return new ScreenShotMaker(Driver); }
        }

        public PostcodeProvider Postcodes = new PostcodeProvider();
        protected static readonly string BaseUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"];
    }
}
