using OpenQA.Selenium;

namespace NHS111.Web.Functional.Utils
{
    public class OutOfAreaPage : LayoutPage, ISubmitPostcodeResult
    {

        public OutOfAreaPage(IWebDriver driver)
            : base(driver) { }

        public bool ValidationVisible()
        {
            return false;
        }
    }
}
