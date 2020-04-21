using OpenQA.Selenium;

namespace NHS111.Web.Functional.Utils
{
    public class Expert24Page : LayoutPage, ISubmitPostcodeResult
    {

        public Expert24Page(IWebDriver driver)
            : base(driver) { }

        public bool ValidationVisible()
        {
            return false;
        }
    }
}
