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
    public class PreOutcomePage : DispositionPage<PreOutcomePage>
    {
        [FindsBy(How = How.Id, Using = "FindService_CurrentPostcode")]
        private IWebElement PostcodeField { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h2")]
        private IWebElement SubHeader { get; set; }

        [FindsBy(How = How.CssSelector, Using = "button[name='service']")]
        private IWebElement ShowServices { get; set; }

        public PreOutcomePage(IWebDriver driver) : base(driver)
        {
        }

        public override PreOutcomePage EnterPostCodeAndSubmit(string postcode)
        {
            PostcodeField.Clear();
            PostcodeField.SendKeys(postcode);
            PostcodeSubmitButton.Click();
            return new PreOutcomePage(Driver);
        }

        public RecommendedServicePage ClickShowServices()
        {
            ShowServices.Click();
            return new RecommendedServicePage(Driver);
        }

        public PreOutcomePage CompareAndVerify(string uniqueId)
        {
            return base.CompareAndVerify(this, uniqueId);
        }

        public void VerifyHeader()
        {
            Assert.IsTrue(Header.Displayed, "Possible unexpected service result. Expected header to exist but it doesn't.");
            Assert.AreEqual("We’ve found services that can help", Header.Text);
        }
        public void VerifySubHeader()
        {
            Assert.IsTrue(SubHeader.Displayed);
            Assert.AreEqual("Pay standard NHS charges with a referral", SubHeader.Text);
        }

        public void VerifyShowServicesButton()
        {
            Assert.IsTrue(ShowServices.Displayed);
            Assert.AreEqual("Show me services that can help", ShowServices.Text);
        }
    }
}
