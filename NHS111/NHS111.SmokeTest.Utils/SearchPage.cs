using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.SmokeTest.Utils
{
    public class SearchPage : LayoutPage
    {
        private const string _headerText = "Tell us the symptom you’re concerned about";
        private const string _noInputValidationText = "Please enter the symptom you're concerned about";
        private const string _categoriesLinkText = "searching by category";
        public string _invalidSearchText = "a";

        [FindsBy(How = How.Id, Using = "SanitisedSearchTerm")]
        private IWebElement SearchTxtBox { get; set; }

        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NextButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".form-group h1 label")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span[data-valmsg-for='SanitisedSearchTerm']")]
        private IWebElement SearchTxtBoxValidationMessage { get; set; }

        [FindsBy(How = How.Id, Using = "show-categories")]
        private IWebElement CategoriesLink { get; set; }

        public SearchPage(IWebDriver driver) : base(driver)
        {
        }

        public void TypeSearchTextAndClickGo()
        {
            TypeSearchTextAndSelect("Headache");
            ClickNextButton();
        }

        public void VerifyHeader()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

        public QuestionPage TypeSearchTextAndSelect(string pathway)
        {
            SearchTxtBox.Clear();
            SearchTxtBox.SendKeys(pathway);
            this.ClickNextButton();
            new WebDriverWait(Driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li")));
            Driver.FindElement(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li/a[@data-title='" + pathway + "']")).Click();
            return new QuestionPage(Driver);
        }

        public CategoryPage TypeInvalidSearch()
        {
            SearchByTerm(_invalidSearchText);
            return new CategoryPage(Driver);
        }

        public IEnumerable<IWebElement> GetHits()
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li")));
            return Driver.FindElements(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li")); 
        }

        public void SearchByTerm(string term)
        {
            SearchTxtBox.Clear();
            SearchTxtBox.SendKeys(term);
            ClickNextButton();
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

        public void VerifyTabbingOrder(string searchTerm)
        {
            HeaderLogo.SendKeys(Keys.Tab);
            var feedbackLink = Driver.SwitchTo().ActiveElement();
            feedbackLink.SendKeys(Keys.Tab);
            var searchTxtBox = Driver.SwitchTo().ActiveElement();
            searchTxtBox.SendKeys(searchTerm);
            searchTxtBox.SendKeys(Keys.Tab);
            var nextButtonElement = Driver.SwitchTo().ActiveElement();
            nextButtonElement.SendKeys(Keys.Enter);
            //Page Loads Results, so the elements have been recreated
            //on the new page, so we must get it again.
            HeaderLogo.SendKeys(Keys.Tab);
            var feedbackLink2 = Driver.SwitchTo().ActiveElement();
            feedbackLink2.SendKeys(Keys.Tab);
            var firstSearchResultLink = Driver.SwitchTo().ActiveElement();

            Assert.AreEqual(searchTerm.ToLower(), firstSearchResultLink.Text.ToLower());
        }
        
        public void VerifyNoInputValidation()
        {
            Assert.IsTrue(SearchTxtBoxValidationMessage.Displayed);
            Assert.AreEqual(_noInputValidationText, SearchTxtBoxValidationMessage.Text);
        }

        public void VerifyCategoriesLinkPresent()
        {
            Assert.IsTrue(CategoriesLink.Displayed);
            Assert.AreEqual(_categoriesLinkText, CategoriesLink.Text);

        }

        private QuestionPage ClickNextButton()
        {
            NextButton.Click();
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
