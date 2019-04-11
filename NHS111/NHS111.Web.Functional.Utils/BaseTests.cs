﻿using System;
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
                chromeOptions.AddArgument("--headless");
            }
            chromeOptions.AddArgument("--window-size=1232,1000"); // Ensure all screenshots are same size across build agents
            chromeOptions.AddArgument("--no-sandbox"); //https://stackoverflow.com/a/50725918/1689770
            chromeOptions.AddArgument("--disable-infobars"); //https://stackoverflow.com/a/43840128/1689770
            chromeOptions.AddArgument("--disable-dev-shm-usage"); //https://stackoverflow.com/a/50725918/1689770
            chromeOptions.AddArgument("--disable-browser-side-navigation"); //https://stackoverflow.com/a/49123152/1689770
            chromeOptions.AddArgument("--disable-gpu"); //https://stackoverflow.com/questions/51959986/how-to-solve-selenium-chromedriver-timed-out-receiving-message-from-renderer-exc
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
