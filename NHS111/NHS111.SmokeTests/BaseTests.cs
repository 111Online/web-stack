using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NHS111.SmokeTests
{
    public class BaseTests
    {
        public IWebDriver Driver;

        [TestFixtureSetUp]
        public void InitTests()
        {
            Driver = new ChromeDriver();
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
    }
}
