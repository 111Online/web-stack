using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class PageNotFound : LayoutPage
    {
        private static string _errorTitleCode = "Error 404";
        private static string _errorTitle = "Sorry, there is a problem.";

        [FindsBy(How = How.CssSelector, Using = "main[id='content'] > div.page-section > h1.question-header")]
        internal IWebElement PageTitle { get; set; }

        public PageNotFound(IWebDriver driver) : base(driver)
        {
            Load();
        }

        public void Load()
        {
            Uri uri = new Uri(Driver.Url);

            Driver.Navigate().GoToUrl(uri.Scheme + "://" + uri.Host + "/this_doesnt_exist" + uri.Query);
        }

        public void Verify()
        {
            Assert.IsTrue(PageTitle.Displayed);
            Assert.AreEqual(_errorTitle,PageTitle.Text);
            Assert.IsTrue(Driver.Title.Contains(_errorTitleCode));
        }
    }
}
