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
    public class DispositionPage
    {
        private readonly IWebDriver _driver;

        [FindsBy(How = How.CssSelector, Using = ".outcome-header h2")]
        public IWebElement Header { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".outcome-header h3")]
        public IWebElement SubHeader { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".sub-header p")]
        public IWebElement HeaderOtherInfo { get; set; }

        [FindsBy(How = How.ClassName, Using = "worsening-advice-icon")]
        public IWebElement WhatIfFeelWorsePanel { get; set; }

        [FindsBy(How = How.ClassName, Using = "self-care-title")]
        public IWebElement CareAdviceTitleElement { get; set; }


        [FindsBy(How = How.ClassName, Using = "findservice")]
        public IWebElement FindServicePanel { get; set; }

        [FindsBy(How = How.Id, Using = "UserInfo_CurrentAddress_Postcode")]
        public IWebElement PostcodeField { get; set; }

        [FindsBy(How = How.Id, Using = "DosLookup")]
        public IWebElement PostcodeSubmitButton { get; set; }


        public DispositionPage EnterPostCodeAndSubmit(string postcode)
        {
            this.PostcodeField.SendKeys(postcode);
            this.PostcodeSubmitButton.Click();
            return new DispositionPage(_driver);
        }


        public DispositionPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public void VerifySubHeader(string subHeadertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(subHeadertext, SubHeader.Text);
        }

        public void VerifyWorseningPanel(WorseningMessageType messageType)
        {
            Assert.IsTrue(WhatIfFeelWorsePanel.Displayed);
            Assert.AreEqual(messageType.Value, WhatIfFeelWorsePanel.Text);
        }

        public void VerifyOutcome(string outcomeHeadertext)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(outcomeHeadertext, Header.Text);
        }

        public void VerifyHeaderOtherInfo(string otherInfoHeadertext)
        {
            Assert.IsTrue(HeaderOtherInfo.Displayed);
            Assert.AreEqual(otherInfoHeadertext, HeaderOtherInfo.Text);
        }

        public void VerifyFindService(FindServiceType serviceType)
        {
            Assert.IsTrue(FindServicePanel.Displayed);
            Assert.AreEqual(serviceType.Headertext, FindServicePanel.FindElement(By.TagName("h3")).Text);
        }

        public void VerifyCareAdviceHeader(string careAdciceTitle)
        {
            Assert.IsTrue(CareAdviceTitleElement.Displayed);
            Assert.AreEqual(careAdciceTitle, CareAdviceTitleElement.Text);
        }

        public void VerifyCareAdvice(string[] expectedAdviceItems)
        {
            var foundItems = _driver.FindElements(By.CssSelector(".care-advice div h4.self-care-title"));
            Assert.AreEqual(expectedAdviceItems.Count(), foundItems.Count);

            foreach (var item in foundItems)
            {
                Assert.IsTrue(expectedAdviceItems.Contains(item.Text));
            }
        }
    }

    public static class WorseningMessages
    {
        public static WorseningMessageType Call999 = new WorseningMessageType("If there are any new symptoms, or if the condition gets worse, changes or you have any other concerns, call NHS 111 for advice. Calls to 111 are free.");

        public static WorseningMessageType Call111 = new WorseningMessageType("If there are any new symptoms, or if the condition gets worse, changes or you have any other concerns, call NHS 111 for advice. Calls to 111 are free.");
    }

    public class WorseningMessageType
    {
        public WorseningMessageType(string worseningText)
        {
            _worseningText = worseningText;
        }
        private string _worseningText;
        public string Value{ get { return _worseningText; }}
    }

    public static class FindServiceTypes
    {
        public static FindServiceType AccidentAndEmergency = new FindServiceType("Find a service that can see you");
        public static FindServiceType Pharmacy = new FindServiceType("Find a pharmacy");
        public static FindServiceType SexualHealthClinic = new FindServiceType("Find a sexual health clinic");
        public static FindServiceType EmergencyDental = new FindServiceType("Find an emergency dental service that can see you");
        public static FindServiceType Optician = new FindServiceType("Find an optician");
        public static FindServiceType Dental = new FindServiceType("Find a dental service");
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
