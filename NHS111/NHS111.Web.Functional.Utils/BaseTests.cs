using System;
using System.Configuration;
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
        public void InitTests()
        {
            Driver = new ChromeDriver();
            // Ideally we could have multiple size screenshots
            // for Visual Regression Test MVP this uses the same width as Andria's Selenium screenshots (1232px)
            Driver.Manage().Window.Size = new Size(1232, 1000);
        }

        [TestFixtureTearDown]
        public void TeardownTest()
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
        public PostcodeProvider Postcodes = new PostcodeProvider();
        protected static readonly string BaseUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"];
    }
}
