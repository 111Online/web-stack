using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;

namespace NHS111.Web.Functional.Utils
{
    public class CovidHomePage : LayoutPage
    {
        [FindsBy(How=How.CssSelector, Using = "h1.nhsuk-heading-xl")]
        private IWebElement Title { get; set; }

        [FindsBy(How=How.Id, Using = "start-symptom-checker")]
        private IWebElement StartNowButton { get; set; }

        [FindsBy(How = How.Id, Using = "start-symptom-checker-link")]
        private IWebElement StartNowLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = "a[href*='nhs.uk/conditions/coronavirus-covid-19']")]
        private IWebElement NHSUKLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = "a[data-event-value='Get an isolation note']")]
        private IWebElement IsoNoteLink { get; set; }

        public CovidHomePage(IWebDriver driver) : base(driver)
        {
        }

        public CovidHomePage Visit()
        {
            var baseUri = new Uri(_baseUrl + "/service/covid-19");
            Driver.Navigate().GoToUrl(baseUri);
            return this;
        }

        public void VerifyCovidPathway()
        {
            VerifyHiddenField("PathwayNo", "PW1851");
        }

        public void VerifyStartNowButton()
        {
             Assert.IsTrue(StartNowButton.Displayed);
        }

        public void VerifyStartNowLink()
        {
            Assert.IsTrue(StartNowLink.Displayed);
        }

        public void VerifyNHSUKLink()
        {
            Assert.IsTrue(NHSUKLink.Displayed);
        }

        public void VerifyIsoNoteLink()
        {
            Assert.IsTrue(IsoNoteLink.Displayed);
        }

        public LocationPage ClickOnStartNow()
        {
            StartNowButton.Click();
            return new LocationPage(Driver);
        }
    }
}