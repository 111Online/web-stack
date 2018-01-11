using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.SmokeTest.Utils
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
    }
}
