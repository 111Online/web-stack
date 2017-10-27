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
    public class QuestionPage : LayoutPage
    {
        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NextButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.ClassName, Using = "callout")]
        [FindsBy(How = How.ClassName, Using = "callout--info")]
        private IWebElement Rationale { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[for='Yes']")]
        private IWebElement AnswerYesButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[for='No']")]
        private IWebElement AnswerNoButton { get; set; }

        public QuestionPage(IWebDriver driver) : base(driver)
        {
        }

        public void VerifyQuestion(string expectedQuestion)
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(expectedQuestion, Header.Text);
        }

        public void VerifyRationale()
        {
            Assert.IsTrue(Rationale.Displayed);
        }

        public QuestionPage AnswerAndVerifyQuestion(int answerOrder, string expectedQuestion, bool requireButtonAwait = true)
        {
            Driver.FindElement(By.XPath("(//label[contains(@class, 'multiple-choice--radio')])[" + answerOrder + "]")).Click();
            NextButton.Click();
            AwaitNextQuestionPage(requireButtonAwait);
            Assert.AreEqual(expectedQuestion, Header.Text);
            return new QuestionPage(Driver);
        }

        public QuestionPage AnswerYes()
        {
            if (!Driver.FindElements(By.CssSelector("[for='Yes']")).Any())
                throw new Exception("No answer with label for 'Yes' could be found for question " + Header.Text);

            AnswerYesButton.Click();
            NextButton.Click();
            AwaitNextQuestionPage();
            return new QuestionPage(Driver);
        }

        public QuestionPage Answer(string answerText)
        {
            Driver.FindElement(By.XPath("//label[contains(@class, 'multiple-choice--radio') and text() = \"" + answerText + "\"]")).Click();
            NextButton.Click();
            AwaitNextQuestionPage();
            return new QuestionPage(Driver);
        }

        public QuestionPage Answer(int answerOrder, bool requireButtonAwait = true)
        {
            Driver.FindElement(By.XPath("(//label[contains(@class, 'multiple-choice--radio')])[" + answerOrder + "]")).Click();
            NextButton.Click();
            AwaitNextQuestionPage(requireButtonAwait);
            return new QuestionPage(Driver);
        }

        public T AnswerForDispostion<T>(string answerText) where T : DispositionPage<T>
        {
            Driver.FindElement(By.XPath("//label[contains(@class, 'multiple-choice--radio') and text() = \"" + answerText + "\"]")).Click();
            NextButton.Click();
            return (T) Activator.CreateInstance(typeof(T), Driver);
        }

        public T AnswerForDispostion<T>(int answerOrder) where T : DispositionPage<T>
        {
            Driver.FindElement(By.XPath("(//label[contains(@class, 'multiple-choice--radio')])[" + answerOrder + "]")).Click();
            NextButton.Click();
            return (T)Activator.CreateInstance(typeof(T), Driver);
        }

        public QuestionPage AnswerSuccessiveByOrder(int answerOrder, int numberOfTimes)
        {
            int i = 0;
            var questionPage = this;
            while (i < numberOfTimes)
            {
                questionPage = questionPage.Answer(answerOrder);
                i++;
            }
            return questionPage;
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

        public QuestionPage AnswerNo(bool requireButtonAwait = true)
        {
            if (!Driver.FindElements(By.Id("No")).Any())
                throw new Exception("No answer with id 'No' could be found for question " + Header.Text);

            AnswerNoButton.Click();
            NextButton.Click();
            AwaitNextQuestionPage(requireButtonAwait);
            return new QuestionPage(Driver);
        }

        private void AwaitNextQuestionPage(bool requireButtonAwait = true)
        {
            if(requireButtonAwait)
                new WebDriverWait(Driver, TimeSpan.FromSeconds(20)).Until(
                ExpectedConditions.ElementExists(By.CssSelector(".multiple-choice--radio")));
            new WebDriverWait(Driver, TimeSpan.FromSeconds(1));
        }

        public QuestionPage NavigateBack()
        {
            Driver.Navigate().Back();
            return new QuestionPage(Driver);
        }

        public void VerifyQuestionPageLoaded()
        {
            Assert.AreEqual("Next question", NextButton.Text);
            Assert.IsTrue(NextButton.Displayed);
        }
    }
}
