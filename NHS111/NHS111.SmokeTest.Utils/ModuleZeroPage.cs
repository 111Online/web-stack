using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.SmokeTest.Utils
{
    public class ModuleZeroPage : LayoutPage
    {
        private const string _headerText = "Do any of these apply?";
        private const string _firstExpandableLinkHiddenText = "A feeling of crushing pressure like a heavy weight pushing down on your chest.";

        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NoneApplyButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h1.heading-large")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.XPath, Using = "//summary[1]")]
        private IWebElement FirstExpandableLink { get; set; }

        [FindsBy(How = How.Id, Using = "details-content-0")]
        private IWebElement FirstExpandableLinkExpanded { get; set; }

        public ModuleZeroPage(IWebDriver driver) : base(driver)
        {
        }

        public DemographicsPage ClickNoneApplyButton()
        {
            NoneApplyButton.Submit();
            return new DemographicsPage(Driver);
        }

        public void VerifyHeader()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

        public void VerifyExpandableLink()
        {
            FirstExpandableLink.Click();
            Assert.AreEqual(_firstExpandableLinkHiddenText, FirstExpandableLinkExpanded.Text);
            Assert.IsTrue(FirstExpandableLinkExpanded.Displayed);
        }
    }
}
