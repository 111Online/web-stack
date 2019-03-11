using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class PostcodeFirstPage : DispositionPage<PostcodeFirstPage>
    {
        [FindsBy(How = How.Id, Using = "UserInfo_CurrentAddress_Postcode")]
        private IWebElement PostcodeField { get; set; }

        public PostcodeFirstPage(IWebDriver driver) : base(driver)
        {
        }

        public override PostcodeFirstPage EnterPostCodeAndSubmit(string postcode)
        {
            PostcodeField.Clear();
            PostcodeField.SendKeys(postcode);
            PostcodeSubmitButton.Click();
            return new PostcodeFirstPage(Driver);
        }

        public PostcodeFirstPage CompareAndVerify(string uniqueId)
        {
            return base.CompareAndVerify(this, uniqueId);
        }
    }
}
