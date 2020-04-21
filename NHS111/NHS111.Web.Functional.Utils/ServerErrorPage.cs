using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class ServerErrorPage : LayoutPage
    {
        private static string _errorTitleCode = "Error 500";
        private static string _errorTitle = "Sorry, there is a problem with the service";

        [FindsBy(How = How.CssSelector, Using = "main[id='content'] > div.page-section > h1.question-header")]
        internal IWebElement PageTitle { get; set; }

        public ServerErrorPage(IWebDriver driver) : base(driver)
        {

        }


        public void Verify()
        {
            Assert.IsTrue(PageTitle.Displayed);
            Assert.AreEqual(_errorTitle, PageTitle.Text);
            Assert.IsTrue(Driver.Title.Contains(_errorTitleCode));
        }
    }
}
