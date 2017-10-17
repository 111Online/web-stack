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

        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NoneApplyButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h1.heading-large")]
        private IWebElement Header { get; set; }

        public ModuleZeroPage(IWebDriver driver) : base(driver)
        {
        }

        public DemographicsPage ClickNoneApplyButton()
        {
            NoneApplyButton.Submit();
            return new DemographicsPage(Driver);
        }
        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

    }
}
