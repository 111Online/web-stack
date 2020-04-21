using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class PageNotFound : LayoutPage
    {
        private static string _errorTitleCode = "Error 404";
        private static string _errorTitle = "Page not found";

        [FindsBy(How = How.CssSelector, Using = "main[id='content'] > div.page-section > h1.question-header")]
        internal IWebElement PageTitle { get; set; }

        public PageNotFound(IWebDriver driver) : base(driver)
        {
            Load();
        }

        public void Load()
        {
            Driver.Navigate().GoToUrl(_baseUrl + "/this_doesnt_exist");
        }

        public void Verify()
        {
            Assert.IsTrue(PageTitle.Displayed);
            Assert.AreEqual(_errorTitle, PageTitle.Text);
            Assert.IsTrue(Driver.Title.Contains(_errorTitleCode));
        }
    }
}
