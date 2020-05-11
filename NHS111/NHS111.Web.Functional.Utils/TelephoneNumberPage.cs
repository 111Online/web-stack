using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Features;
using NUnit.Framework;
using OpenQA.Selenium;

namespace NHS111.Web.Functional.Utils
{
    public class TelephoneNumberPage: LayoutPage
    {
        public TelephoneNumberPage(IWebDriver driver) : base(driver)
        {
        }

        public TelephoneNumberPage(IWebDriver driver, IVisualRegressionTestingFeature visualRegressionTestingFeature) : base(driver, visualRegressionTestingFeature)
        {
        }


        public void EnterPhoneNumber(string phone)
        {
            Driver.FindElement(By.Id("TelephoneNumber")).SendKeys(phone);
        }


        public void VerifyIsTelephoneNumberPage()
        {
            VerifyHeading("What number should we call?");
        }


        public void VerifyHeading(string headertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(headertext, Header.Text);
        }


        public PersonalDetailsPage SubmitTelephoneNumber()
        {
            Driver.FindElement(By.Id("submitDetails")).Click();
            return new PersonalDetailsPage(Driver);
        }
    }
}
