using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class VerifySMSPage : LayoutPage
    {
        [FindsBy(How = How.Id, Using= "book-call")]
        public IWebElement SendCodeButton { get; set; }
        
        [FindsBy(How = How.ClassName, Using = "nhsuk-heading-l")]
        public IWebElement HeadingElement { get; set; }



        public VerifySMSPage(IWebDriver driver)
            : base(driver)
        {
        }

        public EnterVerificationCodeSMSPage Submit()
        {
            SendCodeButton.Click();
            return new EnterVerificationCodeSMSPage(Driver);
        }

        public void VerifyPageContent()
        {
            Assert.AreEqual("Get a security code", HeadingElement.Text);
            Assert.AreEqual("Send a code to my phone", SendCodeButton.Text);
        }

    }
}
