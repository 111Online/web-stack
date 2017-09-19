﻿using System;
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

        [FindsBy(How = How.ClassName, Using = "button--next")]
        public IWebElement NextButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "h1")]
        public IWebElement Header { get; set; }

        [FindsBy(How = How.ClassName, Using = "button--previous")]
        public IWebElement PreviousButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[for='Yes']")]
        public IWebElement AnswerYesButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[for='No']")]
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
            if (!_driver.FindElements(By.CssSelector("[for='Yes']")).Any())
                throw new Exception("No answer with label for 'Yes' could be found for question " + Header.Text);

            AnswerYesButton.Click();
            NextButton.Click();
            AwaitNextQuestionPage();
            return new QuestionPage(_driver);
        }

        public QuestionPage Answer(string answerText)
        {
            _driver.FindElement(By.XPath("//label[contains(@class, 'multiple-choice--radio') and text() = \"" + answerText + "\"]")).Click();
            NextButton.Click();
            AwaitNextQuestionPage();
            return new QuestionPage(_driver);
        }

        public QuestionPage Answer(int answerOrder, bool requireButtonAwait = true)
        {
            _driver.FindElement(By.XPath("(//label[contains(@class, 'multiple-choice--radio')])[" + answerOrder + "]")).Click();
            NextButton.Click();
            AwaitNextQuestionPage(requireButtonAwait);
            return new QuestionPage(_driver);
        }

        public DispositionPage AnswerForDispostion(string answerText)
        {
            _driver.FindElement(By.XPath("//label[contains(@class, 'multiple-choice--radio') and text() = \"" + answerText + "\"]")).Click();
            NextButton.Click();
            return new DispositionPage(_driver);
        }

        public DispositionPage AnswerForDispostion(int answerOrder)
        {
            _driver.FindElement(By.XPath("(//label[contains(@class, 'multiple-choice--radio')])[" + answerOrder + "]")).Click();
            NextButton.Click();
            return new DispositionPage(_driver);
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
            if (!_driver.FindElements(By.Id("No")).Any())
                throw new Exception("No answer with id 'No' could be found for question " + Header.Text);

            AnswerNoButton.Click();
            NextButton.Click();
            AwaitNextQuestionPage(requireButtonAwait);
            return new QuestionPage(_driver);
        }

        private void AwaitNextQuestionPage(bool requireButtonAwait = true)
        {
            if(requireButtonAwait)
                new WebDriverWait(_driver, TimeSpan.FromSeconds(20)).Until(
                ExpectedConditions.ElementExists(By.CssSelector(".multiple-choice--radio")));
            new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
        }

        public QuestionPage NavigateBack()
        {
            _driver.Navigate().Back();
            return new QuestionPage(_driver);
        }
    }
}
