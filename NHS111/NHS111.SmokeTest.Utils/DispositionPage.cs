using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.SmokeTest.Utils
{
    public abstract class DispositionPage<T> : LayoutPage
    {
        private const string PATHWAY_NOT_FOUND__EXPECTED_TEXT = "Call 111 to speak to an advisor now";

        [FindsBy(How = How.CssSelector, Using = ".local-header h1")]
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

        [FindsBy(How = How.CssSelector, Using = ".care-advice .heading-medium")]
        private IWebElement CareAdviceTitleElement { get; set; }

        [FindsBy(How = How.ClassName, Using = "findservice-form")]
        private IWebElement FindServicePanel { get; set; }

        [FindsBy(How = How.Id, Using = "DosLookup")]
        public IWebElement PostcodeSubmitButton { get; set; }

        protected DispositionPage(IWebDriver driver) : base(driver)
        {
        }

        public abstract T EnterPostCodeAndSubmit(string postcode);

        public QuestionPage NavigateBack()
        {
            Driver.Navigate().Back();
            return new QuestionPage(Driver);
        }

        public DemographicsPage NavigateBackToGenderPage()
        {
            while (Driver.Title != "NHS 111 Online - Tell us about you")
            {
                Driver.Navigate().Back();
            }
            return new DemographicsPage(Driver);
        }

        public void VerifySubHeader(string subHeadertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(subHeadertext, SubHeader.Text);
        }

        public void VerifyWorseningPanel(WorseningMessageType messageType)
        {
            Assert.IsTrue(WhatIfFeelWorsePanel.Displayed);
            if(!String.IsNullOrWhiteSpace(messageType.HeaderText)) Assert.AreEqual(messageType.HeaderText, WhatIfFeelWorseHeader.Text);
            Assert.AreEqual(messageType.Value, WhatIfFeelWorsePanel.Text);
        }

        public void VerifyOutcome(string outcomeHeadertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(outcomeHeadertext, Header.Text);
        }

        public void VerifyDispositionCode(string dispositionCode)
        {
            bool result = true;
            var xpath = string.Format("//input[@value = \"{0}\"]", dispositionCode);
            IWebElement dispostionCodeField = null;
            try
            {
                dispostionCodeField = Driver.FindElement(By.XPath(xpath));
            }
            catch (NoSuchElementException)
            {
                result = false;
            }
            Assert.IsTrue(result, string.Format("VerifyDispositionCode : {0}", xpath));
            Assert.AreEqual(dispositionCode, dispostionCodeField.GetAttribute("value"));
        }

        public void VerifyPathwayNotFound()
        {
            Assert.IsTrue(PathwayNotFoundHeader.Displayed);
            Assert.AreEqual(PATHWAY_NOT_FOUND__EXPECTED_TEXT, PathwayNotFoundHeader.Text);
        }

        public void VerifyHeaderOtherInfo(string otherInfoHeadertext)
        {
            Assert.IsTrue(HeaderOtherInfo.Displayed);
            Assert.AreEqual(otherInfoHeadertext, HeaderOtherInfo.Text);
        }

        public void VerifyFindService(FindServiceType serviceType)
        {
            Assert.IsTrue(FindServicePanel.Displayed);
            Assert.AreEqual(serviceType.Headertext, FindServicePanel.FindElement(By.TagName("h2")).Text);
        }

        public void VerifyCareAdviceHeader(string careAdciceTitle)
        {
            Assert.IsTrue(CareAdviceTitleElement.Displayed);
            Assert.AreEqual(careAdciceTitle, CareAdviceTitleElement.Text);
        }

        public void VerifyCareAdvice(string[] expectedAdviceItems)
        {
            var foundItems = Driver.FindElements(By.CssSelector(".care-advice div h3"));
            Assert.AreEqual(expectedAdviceItems.Count(), foundItems.Count);

            foreach (var item in foundItems)
            {
                Assert.IsTrue(expectedAdviceItems.Contains(item.Text));
            }
        }
    }

    public static class WorseningMessages
    {
        public static WorseningMessageType Call999 = new WorseningMessageType("If there are any new symptoms, or if the condition gets worse, call 111 for advice.");

        public static WorseningMessageType Call111 = new WorseningMessageType("If there are any new symptoms, or if the condition gets worse, call 111 for advice.");
        public static WorseningMessageType Call111PostCodeFirst = new WorseningMessageType("If there are any new symptoms, or if the condition gets worse, call 111 for advice.", "Call 111 if your symptoms get worse");
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
