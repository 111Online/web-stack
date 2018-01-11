using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.SmokeTest.Utils
{
    public class CategoryPage : LayoutPage
    {
        private const string _topicsMessageText = "Try finding your symptoms by topic instead. If you can't find what you need, please call 111 now.";

        [FindsBy(How = How.XPath, Using = "//*[@id=\"categories\"]/h2[1]")]
        private IWebElement NoResultsMessage { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"categories\"]/h2[2]")]
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
            Assert.AreEqual("Sorry, there are no results for '" + searchTerm + "'.", NoResultsMessage.Text);
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

        public void VerifyTabbingOrder(string topicToSelect1, string topicToSelect2, string topicToSelect3)
        {
            HeaderLogo.SendKeys(Keys.Tab);
            var feedbackLink = Driver.SwitchTo().ActiveElement();
            feedbackLink.SendKeys(Keys.Tab);
            var topic1 = Driver.SwitchTo().ActiveElement();
            Assert.IsTrue(topic1.Text.Contains(topicToSelect1));
            topic1.SendKeys(Keys.Enter);
            topic1 = Driver.SwitchTo().ActiveElement();
            topic1.SendKeys(Keys.Tab);
            var topic2 = Driver.SwitchTo().ActiveElement();
            Assert.IsTrue(topic2.Text.Contains(topicToSelect2));
            topic2.SendKeys(Keys.Enter);
            topic2 = Driver.SwitchTo().ActiveElement();
            topic2.SendKeys(Keys.Tab);
            var topic3 = Driver.SwitchTo().ActiveElement();
            Assert.IsTrue(topic3.Text.Contains(topicToSelect3));
            topic3.SendKeys(Keys.Enter);
            
            QuestionPage questionPage = new QuestionPage(Driver);
            questionPage.VerifyQuestionPageLoaded();
        }

        public void SelectCategory(string categoryTitle)
        {
            new WebDriverWait(Driver, new TimeSpan(0, 0, 5))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.Id(categoryTitle))));
            Driver.FindElement(By.Id(categoryTitle)).Click();
        }

        public void SelectPathway(string pathwayTitle)
        {
            new WebDriverWait(Driver, new TimeSpan(0, 0, 5))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.XPath(String.Format("//a[@data-title= '{0}']", pathwayTitle)))));
            Driver.FindElement(By.XPath(String.Format("//a[@data-title= '{0}']", pathwayTitle))).Click();
        }
    }
}
