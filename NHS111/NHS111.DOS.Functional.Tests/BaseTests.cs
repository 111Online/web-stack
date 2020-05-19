namespace NHS111.DOS.Functional.Tests
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;

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

        public void SaveScreenAsPNG(string filename)
        {
            var screenshotDriver = Driver as ITakesScreenshot;
            var screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(Path.Combine("C:\\", filename + ".png"));
        }

        protected string EncryptArgs(Dictionary<string, string> args)
        {
            var key = ConfigurationManager.AppSettings["QueryStringEncryptionKey"];
            var bytes = ConfigurationManager.AppSettings["QueryStringEncryptionBytes"];

            var encryptor = new QueryStringEncryptor(key, bytes);
            foreach (var arg in args)
            {
                encryptor.Add(arg.Key, arg.Value);
            }

            return encryptor.ToString();
        }
    }

}
