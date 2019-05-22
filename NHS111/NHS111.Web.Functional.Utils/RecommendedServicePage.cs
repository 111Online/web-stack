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

        [FindsBy(How = How.ClassName, Using = "card__details--times")]
        private IWebElement OpeningTimes { get; set; }

        [FindsBy(How = How.ClassName, Using = "service-details__distance")]
        private IWebElement Distance { get; set; }

        [FindsBy(How = How.ClassName, Using = "nhsuk-action-link__text")]
        private IWebElement ActionLink { get; set; }

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
            Assert.AreEqual("Other services that can help", OtherServices.Text, string.Format("Possible unexpected service result. Expected 'Other services that can help' but was instead '{0}'", OtherServices.Text));
        }

        public void VerifyNoOtherServices()
        {
            Assert.IsFalse(IsDisplayed(OtherServices), "Possible unexpected service result. Expected no other services to exist but it does.");
        }

        public void VerifyOpeningTimes()
        {
            Assert.IsTrue(OpeningTimes.Displayed, "Possible unexpected service result. Expected service details to exist but it doesn't.");
        }

        public void VerifyDistance()
        {
            Assert.IsTrue(Distance.Displayed, "Possible unexpected service result. Expected service details to exist but it doesn't.");
        }

        public void VerifyActionLink(string referText)
        {
            Assert.AreEqual(referText, ActionLink.Text, string.Format("Possible unexpected service result. Expected '{0}' but was instead '{1}'", referText, ActionLink.Text));
        }

        public void VerifyNoActionLink()
        {
            Assert.IsFalse(IsDisplayed(ActionLink), "Possible unexpected service result. Expected service details to not exist but it does.");
        }

        private bool IsDisplayed(IWebElement element)
        {
            try
            {
                return element.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
