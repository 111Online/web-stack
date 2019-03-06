using System;
using System.Configuration;
using NHS111.Web.Functional.Utils.ScreenShot;
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
                var id = TestContext.CurrentContext.Test.GetHashCode();
                ScreenShotMaker.MakeScreenShot(id);
                Console.WriteLine("##teamcity[testMetadata testName='{0}' type='image' value='{1}']", TestContext.CurrentContext.Test.FullName, ScreenShotMaker.GetScreenShotFilename(id));
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
