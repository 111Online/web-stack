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
    public class ModuleZeroPage 
    {
        private readonly IWebDriver _driver;
        private const string _headerText = "Do any of these apply?";

        [FindsBy(How = How.ClassName, Using = "button-next")]
        public IWebElement NoneApplyButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h1.heading-large")]
        public IWebElement Header { get; set; }


        public ModuleZeroPage(IWebDriver driver) 
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public GenderPage ClickNoneApplyButton()
        {
            NoneApplyButton.Submit();
            return new GenderPage(_driver);
        }
        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

    }
}
