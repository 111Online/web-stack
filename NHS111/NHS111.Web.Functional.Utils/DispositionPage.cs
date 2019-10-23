using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    using OpenQA.Selenium.Support.UI;

    public abstract class DispositionPage<T> : LayoutPage {
        private const string PATHWAY_NOT_FOUND__EXPECTED_TEXT = "Call 111 to speak to an adviser now";

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.XPath, Using = "//h1")]
        private IWebElement PathwayNotFoundHeader { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".local-header h3")]
        private IWebElement SubHeader { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".sub-header p")]
        private IWebElement HeaderOtherInfo { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".callout--attention p")]
        private IWebElement WhatIfFeelWorsePanel { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".callout--attention h2")]
        private IWebElement WhatIfFeelWorseHeader { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".care-advice .heading-medium, .care-advice summary")]
        private IWebElement CareAdviceTitleElement { get; set; }

        [FindsBy(How = How.ClassName, Using = "findservice-form")]
        private IWebElement FindServicePanel { get; set; }

        [FindsBy(How = How.Id, Using = "DosLookup")]
        public IWebElement PostcodeSubmitButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "summary")]
        private IList<IWebElement> DOSGroups { get; set; }

        [FindsBy(How = How.ClassName, Using = "cards")]
        private IWebElement DosResults { get; set; }

        protected DispositionPage(IWebDriver driver) : base(driver) { }

        public abstract T EnterPostCodeAndSubmit(string postcode);

        public QuestionPage NavigateBack() {
            Driver.Navigate().Back();
            return new QuestionPage(Driver);
        }

        public DemographicsPage NavigateBackToGenderPage() {
            while (Driver.Title != "NHS 111 Online - Tell us about you") {
                Driver.Navigate().Back();
            }

            return new DemographicsPage(Driver);
        }

        public void VerifySubHeader(string subHeadertext) {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(subHeadertext, SubHeader.Text);
        }

        public void VerifyNoWorseningAdvice() {
            Assert.IsFalse(Driver.ElementExists(By.CssSelector(".callout--attention p")));
        }

        public void VerifyWorseningPanel(WorseningMessageType messageType) {
            Assert.IsTrue(WhatIfFeelWorsePanel.Displayed);
            if (!String.IsNullOrWhiteSpace(messageType.HeaderText))
                Assert.AreEqual(messageType.HeaderText, WhatIfFeelWorseHeader.Text);
            Assert.AreEqual(messageType.Value, WhatIfFeelWorsePanel.Text);
        }

        public void VerifyWorseningReveal(WorseningMessageType messageType) {
            var reveal = Driver.FindElement(By.CssSelector(".worsening-advice summary"));
            Assert.IsTrue(reveal.Displayed);
            if (!String.IsNullOrWhiteSpace(messageType.HeaderText))
                Assert.AreEqual(messageType.HeaderText, reveal.Text);
            if (!String.IsNullOrWhiteSpace(messageType.Value))
                Assert.AreEqual(messageType.Value, reveal.Text);
        }

        public void VerifyOutcome(string outcomeHeadertext) {
            Assert.IsTrue(Driver.ElementExists(By.CssSelector("h1")),
                "Possible unexpected triage outcome. Expected header to exist but it doesn't.");
            Assert.AreEqual(outcomeHeadertext, Header.Text,
                string.Format("Possible unexpected triage outcome. Expected header text of '{0}' but was '{1}'.",
                    outcomeHeadertext, Header.Text));
        }

        public void VerifyOutcome(string outcomeHeadertext1, string outcomeHeadertext2) {
            Assert.IsTrue(Driver.ElementExists(By.CssSelector("h1")),
                "Possible unexpected triage outcome. Expected header to exist but it doesn't.");
            Assert.IsTrue(Header.Text == outcomeHeadertext1 || Header.Text == outcomeHeadertext2,
                string.Format(
                    "Possible unexpected triage outcome. Expected header text of either '{0}' or '{1}' but was '{2}'.",
                    outcomeHeadertext1, outcomeHeadertext2, Header.Text));
        }

        public void VerifyDispositionCode(string dispositionCode) {
            var xpath = "//input[@id='Id']";
            IWebElement dispostionCodeField = null;
            try {
                dispostionCodeField = Driver.FindElement(By.XPath(xpath));
            }
            catch (NoSuchElementException) {
                Assert.Fail(string.Format("No dxcode element found on page. Looking for {0} using xpath {1}",
                    dispostionCodeField, xpath));
            }

            var actualDispoCode = dispostionCodeField.GetAttribute("value");
            Assert.AreEqual(dispositionCode, actualDispoCode,
                string.Format("Expected DxCode {0} but was {1}", dispositionCode, actualDispoCode));
        }

        public void VerifyPathwayNotFound() {
            Assert.IsTrue(PathwayNotFoundHeader.Displayed);
            Assert.AreEqual(PATHWAY_NOT_FOUND__EXPECTED_TEXT, PathwayNotFoundHeader.Text);
        }

        public void VerifyHeaderOtherInfo(string otherInfoHeadertext) {
            Assert.IsTrue(HeaderOtherInfo.Displayed);
            Assert.AreEqual(otherInfoHeadertext, HeaderOtherInfo.Text);
        }

        public void VerifyFindService(FindServiceType serviceType) {
            Assert.IsTrue(FindServicePanel.Displayed);
            Assert.AreEqual(serviceType.Headertext, FindServicePanel.FindElement(By.TagName("h2")).Text);
        }

        public void VerifyCareAdviceHeader(string careAdviceTitle) {
            Assert.IsTrue(CareAdviceTitleElement.Displayed);
            Assert.AreEqual(careAdviceTitle, CareAdviceTitleElement.Text);
        }

        public void VerifyNoCareAdvice() {
            Assert.IsFalse(Driver.ElementExists(By.CssSelector(".care-advice div h4")));
        }

        public void VerifyCareAdvice(string[] expectedAdviceItems) {
            var foundItems = Driver.FindElements(By.CssSelector("[id^='Advice_']"));
            Assert.AreEqual(expectedAdviceItems.Count(), foundItems.Count,
                string.Format("Incorrect number of care advice on disposition '{0}'. Found items were: {1}",
                    Header.Text, foundItems.Select(cx => "'" + cx.Text + "'\n")));

            foreach (var item in foundItems) {
                Assert.IsTrue(expectedAdviceItems.Contains(item.Text));
            }
        }


        public void VerifyIsSuccessfulReferral() {
            Assert.IsTrue(Driver.ElementExists(By.CssSelector("h1")),
                "Possible unexpected triage outcome. Expected header to exist but it doesn't.");
            var header = Driver.FindElement(By.CssSelector("h1"));
            Assert.IsTrue(header.Text.StartsWith("You should get a call within"),
                string.Format(
                    "Possible unexpected triage outcome. Expected header text of 'You should get a call within' but was '{0}'.",
                    header.Text));
        }

        public void VerifyIsUnsuccessfulReferral() {
            VerifyOutcome("Sorry, there is a problem with the service");
        }

        public void VerifyIsServiceUnavailableReferral() {
            VerifyOutcome("Sorry, there is a problem with the service");
        }

        public void VerifyIsDuplicateReferral() {
            VerifyOutcome("Your call has already been booked");
        }

        public bool IsCallbackAcceptancePage()
        {
            Assert.IsTrue(Driver.ElementExists(By.CssSelector("h1")),
                "Possible unexpected triage outcome. Expected header to exist but it doesn't.");
            return Header.Text == "A nurse needs to phone you" || Header.Text == "Get a phone call from a nurse";
        }
        public PersonalDetailsPage AcceptCallback()
        {
            Driver.IfElementExists(By.Id("Yes"), element => element.Click());
            Driver.FindElement(By.Id("next")).Click();
            return new PersonalDetailsPage(Driver);
        }
        public OutcomePage RejectCallback()
        {
            Driver.FindElement(By.Id("No")).Click();
            Driver.FindElement(By.Id("next")).Click();
            return new OutcomePage(Driver);
        }

        // Used for pages with DOS results grouped by type
        public void VerifyPageContainsDOSResults()
        {
            Assert.IsTrue(DOSGroups.Count() > 0, "No DoS result groupings found on page.");
            Assert.IsTrue(DosResults.Displayed);
            var results = DosResults.FindElements(By.ClassName("card"));

            Assert.IsTrue(results.Count > 0, "No DoS results found on page.");
        }

        // VerifyPageContainsDOSServices is used for recommended service and other services pages.
        public void VerifyPageContainsDOSServices()
        {
            Assert.IsTrue(DosResults.Displayed);
            var results = DosResults.FindElements(By.ClassName("service-listing"));

            Assert.IsTrue(results.Count > 0, "No DoS results found on page.");
        }

        public void VerifyDOSResultGroupExists(string groupText)
        {
            Assert.IsTrue(DOSGroups.Any(g => g.Text == groupText));
        }
    }

    public static class WorseningMessages
    {
        public static WorseningMessageType Call999 = new WorseningMessageType("If there are any new symptoms, or if the condition gets worse, call 111 for advice.");

        public static WorseningMessageType Call111 = new WorseningMessageType("If there are any new symptoms, or if the condition gets worse, call 111 for advice.");
        public static WorseningMessageType Call111PostCodeFirst = new WorseningMessageType("If there are any new symptoms, or if the condition gets worse, call 111 for advice.", "Call 111 if your symptoms get worse");
        public static WorseningMessageType PrimaryCare = new WorseningMessageType("What to do if you start to feel worse");

    }

    public class WorseningMessageType
    {
        public WorseningMessageType(string worseningText, string headerText="")
        {
            _worseningText = worseningText;
            _headerText = headerText;
        }
        private string _worseningText;
        private string _headerText;
        public string Value{ get { return _worseningText; }}
        public string HeaderText { get { return _headerText; } }
    }

 


    public static class FindServiceTypes
    {
        public static FindServiceType AccidentAndEmergency = new FindServiceType("Find a service that can see you");
        public static FindServiceType Pharmacy = new FindServiceType("Find a pharmacy");
        public static FindServiceType SexualHealthClinic = new FindServiceType("Find a sexual health clinic");
        public static FindServiceType EmergencyDental = new FindServiceType("Find an emergency dental service that can see you");
        public static FindServiceType Optician = new FindServiceType("Find an optician");
        public static FindServiceType Dental = new FindServiceType("Find a dental service");
        public static FindServiceType Midwife = new FindServiceType("Find a service that can help you");
    }


    public class FindServiceType
    {
        public FindServiceType(string findServiceText)
        {
            _findServiceText = findServiceText;
        }
        private string _findServiceText;
        public string Headertext { get { return _findServiceText; } }
    }
}
