using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.Web.Functional.Utils
{
    public class CategoryPage : LayoutPage
    {
        private const string _topicsMessageText = "All topics";

        [FindsBy(How = How.XPath, Using = "//*[@id=\"SearchResults\"]/h1[1]")]
        private IWebElement NoResultsMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"categories\"]/h1[1]")]
        private IWebElement TopicsMessage { get; set; }

        public CategoryPage(IWebDriver driver) : base(driver)
        {
        }
        public void VerifyHeader()
        {
            Assert.IsTrue(TopicsMessage.Displayed);
            Assert.AreEqual(_topicsMessageText, TopicsMessage.Text);
        }

        public void VerifyNoResultsMessage(string searchTerm)
        {
            Assert.IsTrue(NoResultsMessage.Displayed);
            Assert.AreEqual("Sorry, no topics have been found for '" + searchTerm + "'.", NoResultsMessage.Text);
        }

        public void VerifyCategoryExists(string categoryName)
        {
            Assert.IsTrue(Driver.FindElements(By.ClassName("search__category")).Any(element => element.Text == categoryName));
        }

        public void VerifyCategoryNotExists(string categoryName)
        {
            Assert.IsFalse(Driver.FindElements(By.ClassName("search__category")).Any(element => element.Text == categoryName));
        }

        public void VerifyPathwayInCategoryList(string title, string pathwayId)
        {
            bool result = true;
            var xpath = string.Format("//a[@data-title= \"{0}\"][@data-pathway-number= '{1}']", title, pathwayId);
            try
            {
                Driver.FindElement(By.XPath(xpath));
            }
            catch (NoSuchElementException)
            {
                result = false;
            }
            Assert.IsTrue(result, string.Format("VerifyPathwayInCategoryList : {0}", xpath));
        }

        public void VerifyPathwayNotInCategoryList(string title, string pathwayId)
        {
            bool result = false;
            var xpath = string.Format("//a[@data-title= \"{0}\"][@data-pathway-number= '{1}']", title, pathwayId);
            try
            {
                Driver.FindElement(By.XPath(xpath));
            }
            catch (NoSuchElementException)
            {
                result = true;
            }
            Assert.IsTrue(result, string.Format("VerifyPathwayNotInCategoryList : {0}", xpath));
        }

        public void VerifyTabbingOrder(string topicToSelect) {
            var backLink = TabToFirstPageBodyElement();
            var topic = Driver.TabFrom(backLink);
            Assert.IsTrue(topic.Text.Contains(topicToSelect));
            topic.SendKeys(Keys.Enter);
            
            var questionInfoPage = new QuestionInfoPage(Driver);
            var questionPage = questionInfoPage.ClickIUnderstand();
            questionPage.VerifyQuestionPageLoaded();
        }

        public void SelectCategory(string categoryTitle)
        {
            new WebDriverWait(Driver, new TimeSpan(0, 0, 5))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.Id(categoryTitle))));
            Driver.FindElement(By.Id(categoryTitle)).Click();
        }

        public QuestionInfoPage SelectPathway(string pathwayTitle)
        {
            new WebDriverWait(Driver, new TimeSpan(0, 0, 5))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.XPath(String.Format("//a[@data-title= '{0}']", pathwayTitle)))));
            Driver.FindElement(By.XPath(String.Format("//a[@data-title= '{0}']", pathwayTitle))).Click();
            return new QuestionInfoPage(Driver);
        }

        public void VerifyOtherProblems(string pathwayNumber) {
            Driver.FindElement(By.Id("details-summary-0")).Click();

            bool result = true;
            var xpath = string.Format("//a[@data-pathway-number= '{0}']", pathwayNumber);
            try
            {
                Driver.FindElement(By.XPath(xpath));
            }
            catch (NoSuchElementException)
            {
                result = false;
            }
            Assert.IsTrue(result, string.Format("VerifyPathwayInCategoryList : {0}", xpath));
        }
    }
}
