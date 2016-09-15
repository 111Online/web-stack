using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.SmokeTest.Utils
{
    public class SearchPage
    {
        private readonly IWebDriver _driver;
        private const string _headerText = "I need help with";

        [FindsBy(How = How.Id, Using = "searchTags")]
        public IWebElement SearchTxtBox { get; set; }

        [FindsBy(How = How.ClassName, Using = "button-get-started")]
        public IWebElement GoButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".content-container h2")]
        public IWebElement Header { get; set; }

        public SearchPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public void TypeSearchTextAndClickGo()
        {
            TypeSearchTextAndSelect("Headache");
            ClickGoButton();
        }

        public void Verify()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

        public void TypeSearchTextAndSelect(string pathway)
        {
            SearchTxtBox.Clear();
            SearchTxtBox.SendKeys(pathway);
            new WebDriverWait(_driver, TimeSpan.FromSeconds(20)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//li[contains(@class, 'ui-menu-item') and text() = '" + pathway + "']")));
            _driver.FindElement(By.XPath("//li[contains(@class, 'ui-menu-item') and text() = '" + pathway + "']")).Click();
        }

        public GenderPage ClickGoButton()
        {
            GoButton.Click();
            return new GenderPage(_driver);
        }

    }
}
