using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.Web.Functional.Utils
{
    public class SearchPage : LayoutPage
    {
        private const string _headerText = "Find the right topic";
        private const string _noInputValidationText = "Please enter the symptom you're concerned about";
        private const string _categoriesLinkText = "topics by category.";
        public string InvalidSearchText
        {
            get { return "a"; }
        }
        public const string _errorSearchText = "<a>";

        [FindsBy(How = How.Id, Using = "SanitisedSearchTerm")]
        private IWebElement SearchTxtBox { get; set; }

        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NextButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "main h1 label")]
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

        public QuestionInfoPage TypeSearchTextAndSelect(string pathway)
        {
            SearchTxtBox.Clear();
            SearchTxtBox.SendKeys(pathway);
            this.ClickNextButton();
            new WebDriverWait(Driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li")));
            Driver.FindElement(By.XPath("//ul[contains(@class, 'link-list') and contains(@class, 'link-list--results')]/li/a[@data-title='" + pathway + "']")).Click();
            return new QuestionInfoPage(Driver);
        }

        public CategoryPage TypeInvalidSearch()
        {
            SearchByTerm(InvalidSearchText);
            return new CategoryPage(Driver);
        }

        public ServerErrorPage TypeErrorSearch()
        {
            SearchByTerm(_errorSearchText);
            return new ServerErrorPage(Driver);
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

        // VerifyTermNoHits is used to check filtering works as expected
        public void VerifyTermNoHits(string pathwayNo)
        {
            var elements = Driver.FindElements(By.CssSelector("[data-pathway-number=" + pathwayNo + "]")); 
            Assert.IsTrue(elements.Count == 0);
        }

        public void VerifyTermHits(string expectedHitTitle, int maxRank) {
            expectedHitTitle = expectedHitTitle.ToLower();
            var rank = 0;
            var linkText = "";
            var hits = new List<string>();
            foreach (var hit in this.GetHits().ToList())
            {
                rank++;
                var linkElements = hit.FindElements(By.ClassName("search__topic-title"));
                if (linkElements.Count > 0)
                {
                    linkText = linkElements.FirstOrDefault().Text.StripHTML().ToLower();
                    hits.Add(linkText);
                    if (linkText == expectedHitTitle) break;
                }
            }

            var found = expectedHitTitle == linkText && rank <= maxRank;

            Assert.IsTrue(found, string.Format("Unable to find topic '{0}' within the top {1} search results. Results were {2}", expectedHitTitle, maxRank, string.Join(", ", hits.Select(x => "'" + x + "'"))));
        }

        public void VerifyTabbingOrder(string searchTerm)
        {
            var feedbackLink = Driver.TabFrom(HeaderLogo);
            var searchTxtBox = Driver.TabFrom(feedbackLink);
            searchTxtBox.SendKeys(searchTerm);
            var nextButtonElement = Driver.TabFrom(searchTxtBox);
            nextButtonElement.SendKeys(Keys.Enter);
            //Page Loads Results, so the elements have been recreated
            //on the new page, so we must get it again.
            feedbackLink = Driver.TabFrom(HeaderLogo);
            searchTxtBox = Driver.TabFrom(feedbackLink);
            searchTxtBox.SendKeys(searchTerm);
            nextButtonElement = Driver.TabFrom(searchTxtBox);
            var firstSearchResultLink = Driver.TabFrom(nextButtonElement);

            Assert.IsTrue(firstSearchResultLink.Text.ToLower().StartsWith(searchTerm.ToLower()));
        }
        
        public void VerifyNoInputValidation()
        {
            Assert.IsTrue(SearchTxtBoxValidationMessage.Displayed);
            Assert.AreEqual(_noInputValidationText, SearchTxtBoxValidationMessage.Text);
        }

        public void VerifyCategoriesLinkPresent() {
            Driver.FindElement(By.Id("details-summary-0")).Click();
            var link = Driver.FindElement(By.Id("show-categories"));
            Assert.IsTrue(link.Displayed);
            Assert.AreEqual(_categoriesLinkText, link.Text);

        }

        private QuestionPage ClickNextButton()
        {
            NextButton.Click();
            return new QuestionPage(Driver);
        }

        public CategoryPage ClickCategoryLink() {
            Driver.IfElementExists(By.Id("details-summary-0"), e => e.Click());
            Driver.FindElement(By.Id("show-categories")).Click();
            return new CategoryPage(Driver);
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
