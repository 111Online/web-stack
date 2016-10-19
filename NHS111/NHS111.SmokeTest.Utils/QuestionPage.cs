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
    public class QuestionPage
    {
        private readonly IWebDriver _driver;

        [FindsBy(How = How.ClassName, Using = "button-next")]
        public IWebElement NextButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "question-header")]
        public IWebElement Header { get; set; }
        
        [FindsBy(How = How.ClassName, Using = "previous-question")]
        public IWebElement PreviousButton { get; set; }

        [FindsBy(How = How.Id, Using = "Yes")]
        public IWebElement AnswerYesButton { get; set; }

        [FindsBy(How = How.Id, Using = "No")]
        public IWebElement AnswerNoButton { get; set; }


        public QuestionPage(IWebDriver driver)
        {
            _driver = driver;
            PageFactory.InitElements(_driver, this);
        }

        public QuestionPage ValidateQuestion(string expectedQuestion)
        {
            Assert.AreEqual(expectedQuestion, Header.Text);
            return this;
        }

        public QuestionPage AnswerYes()
        {
            if (!_driver.FindElements(By.Id("Yes")).Any())
                throw new Exception("No answer with id 'Yes' could be found for question " + Header.Text);

            AnswerYesButton.Click();
            NextButton.Click();
            AwaitNextQuestionPage();
            return new QuestionPage(_driver);
        }

        public QuestionPage Answer(string answerText)
        {
            _driver.FindElement(By.XPath("//span[contains(@class, 'answer-text') and text() = '" + answerText + "']/preceding-sibling::span[contains(@class, 'answer-radio')]")).Click();
            NextButton.Click(); 
            AwaitNextQuestionPage();
            return new QuestionPage(_driver);
        }

        public DispositionPage AnswerForDispostion(string answerText)
        {
            _driver.FindElement(By.XPath("//span[contains(@class, 'answer-text') and text() = \"" + answerText + "\"]/preceding-sibling::span[contains(@class, 'answer-radio')]")).Click();
            NextButton.Click();
            return new DispositionPage(_driver);
        }

        public QuestionPage AnswerSuccessiveNo(int numberOfTimes)
        {
            int i = 0;
            var questionPage = this;
            while (i < numberOfTimes)
            {
               questionPage = questionPage.AnswerNo();
               i++;
            }
            return questionPage;
        }

        public QuestionPage AnswerSuccessiveYes(int numberOfTimes)
        {
            int i = 0;
            var questionPage = this;
            while (i < numberOfTimes)
            {
                questionPage = questionPage.AnswerYes();
                i++;
            }
            return questionPage;
        }

        public QuestionPage AnswerNo() {
            if (!_driver.FindElements(By.Id("No")).Any())
                throw new Exception("No answer with id 'No' could be found for question " + Header.Text);

            AnswerNoButton.Click();
            NextButton.Click();
            AwaitNextQuestionPage();
            return new QuestionPage(_driver);
        }

        private void AwaitNextQuestionPage()
        {
            new WebDriverWait(_driver, TimeSpan.FromSeconds(20)).Until(
                ExpectedConditions.ElementExists(By.CssSelector(".button-next:disabled")));
        }
    }
}
