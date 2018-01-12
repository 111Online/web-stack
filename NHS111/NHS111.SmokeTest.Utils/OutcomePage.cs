using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.SmokeTest.Utils
{
    public class OutcomePage : DispositionPage<OutcomePage>
    {
        [FindsBy(How = How.Id, Using = "FindService_UserInfo_CurrentAddress_Postcode")]
        private IWebElement PostcodeField { get; set; }

        public OutcomePage(IWebDriver driver) : base(driver)
        {
        }

        public override OutcomePage EnterPostCodeAndSubmit(string postcode)
        {
            PostcodeField.Clear();
            PostcodeField.SendKeys(postcode);
            PostcodeSubmitButton.Click();
            return new OutcomePage(Driver);
        }
    }
}
