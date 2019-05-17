using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Features;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class RecommendedServicePage : DispositionPage<RecommendedServicePage>
    {
        [FindsBy(How = How.Id, Using = "FindService_CurrentPostcode")]
        private IWebElement PostcodeField { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".callout .callout--attention")]
        private IWebElement Callout { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".callout .callout--attention p a")]
        private IWebElement EnterDetailsLink { get; set; }

        [FindsBy(How = How.ClassName, Using = "service-details")]
        private IWebElement ServiceDetails { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".measure h2")]
        private IWebElement OtherServices { get; set; }

        public RecommendedServicePage(IWebDriver driver) : base(driver)
        {
        }

        public override RecommendedServicePage EnterPostCodeAndSubmit(string postcode)
        {
            PostcodeField.Clear();
            PostcodeField.SendKeys(postcode);
            PostcodeSubmitButton.Click();
            return new RecommendedServicePage(Driver);
        }

        public RecommendedServicePage CompareAndVerify(string uniqueId)
        {
            return base.CompareAndVerify(this, uniqueId);
        }
        public void VerifyCallout()
        {
            Assert.IsTrue(Callout.Displayed, "Possible unexpected service result. Expected callout to exist but it doesn't.");
            Assert.IsTrue(EnterDetailsLink.Displayed, "Possible unexpected service result. Expected 'Enter my details' link");
        }

        public void VerifyServiceDetails()
        {
            Assert.IsTrue(ServiceDetails.Displayed, "Possible unexpected service result. Expected service details to exist but it doesn't.");
        }

        public void VerifyOtherServices()
        {
            Assert.IsTrue(OtherServices.Displayed, "Possible unexpected service result. Expected other services to exist but it doesn't.");
            Assert.AreEqual(OtherServices.Text, "Other services that can help", string.Format("Possible unexpected service result. Expected 'Other services that can help' but was instead '{0}'", OtherServices.Text));
        }
    }
}
