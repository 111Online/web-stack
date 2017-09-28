using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.SmokeTest.Utils
{
    public class SearchPage : LayoutPage
    {
        private const string _headerText = "Tell us the symptom you’re concerned about";

        [FindsBy(How = How.Id, Using = "SanitisedSearchTerm")]
        private IWebElement SearchTxtBox { get; set; }

        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement GoButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".form-group h1 label")]
        private IWebElement Header { get; set; }

        public SearchPage(IWebDriver driver) : base(driver)
        {
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

        public QuestionPage TypeSearchTextAndSelect(string pathway)
        {
            SearchTxtBox.Clear();
            SearchTxtBox.SendKeys(pathway);
            this.ClickGoButton();
            new WebDriverWait(Driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li")));
            Driver.FindElement(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li/a[@data-title='" + pathway + "']")).Click();
            return new QuestionPage(Driver);
        }

        public CategoryPage TypeInvalidSearch()
        {
            SearchByTerm("a");
            return new CategoryPage(Driver);
        }

        public IEnumerable<IWebElement> GetHits()
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li")));
            return Driver.FindElements(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li")); 
        }

        public void SearchByTerm(string term)
        {
            this.SearchTxtBox.Clear();
            this.SearchTxtBox.SendKeys(term);
            this.ClickGoButton();
        }
        public void VerifyTermHits(string expectedHitTitle, int maxRank)
        {
            var rank = 0;
            var linkText = "";
            foreach (var hit in this.GetHits().ToList())
            {
                rank++;
                var linkElements = hit.FindElements(By.TagName("a"));
                if (linkElements.Count > 0)
                {
                    linkText = linkElements.FirstOrDefault().Text.StripHTML().ToLower();
                    if(linkText == expectedHitTitle.ToLower()) break;
                }
            }

            Assert.AreEqual(expectedHitTitle.ToLower(), linkText);
            Assert.IsTrue(rank <= maxRank);
        }


        public QuestionPage ClickGoButton()
        {
            GoButton.Click();
            return new QuestionPage(Driver);
        }

        

    }

    public static class StringExtensionMethods
    {
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}
