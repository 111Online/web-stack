using System.Configuration;
using System.Security;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.SmokeTest.Utils
{
    public class PageNotFound : LayoutPage
    {
        private static string _errorUrl = ConfigurationManager.AppSettings["TestWebsiteUrl"] + "/this_should_not_exist";

        private static string _errorTitleCode = "Error 404";
        private static string _errorTitle = "Sorry, there is a problem.";

        [FindsBy(How = How.CssSelector, Using = "main[id='content'] > h1")]
        private static IWebElement PageTitle { get; set; }

        public PageNotFound(IWebDriver driver) : base(driver)
        {
        }

        public void Load()
        {
            Driver.Navigate().GoToUrl(_errorUrl);
        }

        public void Verify()
        {
            Assert.IsTrue(PageTitle.Displayed);
            Assert.AreEqual(_errorTitle,PageTitle.Text);
            Assert.IsTrue(Driver.Title.Contains(_errorTitleCode));
        }
    }
}
