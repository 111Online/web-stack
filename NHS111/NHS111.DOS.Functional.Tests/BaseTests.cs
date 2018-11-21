namespace NHS111.DOS.Functional.Tests {
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    public class BaseTests
    {
        public IWebDriver Driver;

        public BaseTests()
        {
            Driver = new ChromeDriver();
        }

        ~BaseTests()
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