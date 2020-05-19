using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class AppPage : LayoutPage, ISubmitPostcodeResult
    {

        public AppPage(IWebDriver driver)
            : base(driver) { }

        [FindsBy(How = How.TagName, Using = "h2")]
        private IWebElement AppHeader { get; set; }

        public string AppName
        {
            get { return AppHeader.Text; }
        }

        public bool ValidationVisible()
        {
            return false;
        }
    }
}
