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
    using Newtonsoft.Json.Linq;

    public class QuestionInfoPage
        : LayoutPage {

        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement IUnderstandButton { get; set; }

        public QuestionInfoPage(IWebDriver driver)
            : base(driver) { }

        public QuestionPage ClickIUnderstand() {
            IUnderstandButton.Click();
            return new QuestionPage(Driver);
        }

    }

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

        private const string _containsRadioButton = "contains(@class, 'multiple-choice--radio')";

        public QuestionPage(IWebDriver driver)
            : base(driver) { }

        public QuestionPage AnswerAndVerifyNextQuestion(int answerOrder, string expectedQuestion, bool requireButtonAwait = true) {
            var byOrder = ByOrder(answerOrder);
            SelectAnswerAndSubmit(byOrder, requireButtonAwait);
            VerifyQuestion(expectedQuestion);
            return new QuestionPage(Driver);
        }

        public QuestionPage AnswerYes(bool requireButtonAwait = true) {
            var byForAttribute = By.CssSelector("[for='Yes']");
            return SelectAnswerAndSubmit(byForAttribute, requireButtonAwait);
        }

        public QuestionPage AnswerNo(bool requireButtonAwait = true) {
            var byId = By.Id("No");
            return SelectAnswerAndSubmit(byId, requireButtonAwait);
        }

        public QuestionPage Answer(string answerText, bool requireButtonAwait = true) {
            var byAnswerText = ByAnswerText(answerText);
            return SelectAnswerAndSubmit(byAnswerText, requireButtonAwait);
        }

        public QuestionPage Answer(int answerOrder, bool requireButtonAwait = true) {
            var byOrder = ByOrder(answerOrder);
            return SelectAnswerAndSubmit(byOrder, requireButtonAwait);
        }

        public T AnswerForDeadEnd<T>(string answerText) where T : LayoutPage {
            var byAnswerText = ByAnswerText(answerText);
            SelectAnswerAndSubmit(byAnswerText, false);
            return (T)Activator.CreateInstance(typeof(T), Driver);
        }

        public T AnswerForDispostion<T>(string answerText) where T : DispositionPage<T> {
            var byAnswerText = ByAnswerText(answerText);
            SelectAnswerAndSubmit(byAnswerText, false);
            return (T) Activator.CreateInstance(typeof(T), Driver);
        }

        public T AnswerForDispostion<T>(int answerOrder) where T : DispositionPage<T> {
            var byOrder = ByOrder(answerOrder);
            SelectAnswerAndSubmit(byOrder, false);
            return (T)Activator.CreateInstance(typeof(T), Driver);
        }

        public QuestionPage AnswerSuccessiveByOrder(int answerOrder, int numberOfTimes) {
            var questionPage = this;
            while (numberOfTimes-- > 0)
                questionPage = questionPage.Answer(answerOrder);
            return questionPage;
        }

        public QuestionPage AnswerSuccessiveNo(int numberOfTimes) {
            var questionPage = this;
            while (numberOfTimes-- > 0)
                questionPage = questionPage.AnswerNo();
            return questionPage;
        }

        public QuestionPage AnswerSuccessiveYes(int numberOfTimes) {
            var questionPage = this;
            while (numberOfTimes-- > 0)
                questionPage = questionPage.AnswerYes();
            return questionPage;
        }

        public QuestionPage SelectAnswerAndSubmit(By by, bool expectQuestionPage = true) {
            SelectAnswerBy(by);
            NextButton.Click();
            AwaitNextPage(expectQuestionPage);
            return new QuestionPage(Driver);
        }

        public QuestionPage NavigateBack() {
            Driver.Navigate().Back();
            return new QuestionPage(Driver);
        }

        public void VerifyQuestion(string expectedQuestion) {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(expectedQuestion, Header.Text, string.Format("Unexpected question title. Expected '{0}' but was '{1}'.", expectedQuestion, Header.Text));
        }

        public void VerifyRationale() {
            Assert.IsTrue(Rationale.Displayed);
        }

        public void VerifyQuestionPageLoaded()
        {
            Assert.AreEqual("Next question", NextButton.Text);
            Assert.IsTrue(NextButton.Displayed);
        }

        private static By ByOrder(int answerOrder) {
            return By.XPath(string.Format("(//label[{0}])[{1}]", _containsRadioButton, answerOrder));
        }

        private static By ByAnswerText(string answerText) {
            return By.XPath(string.Format("//label[{0} and text() = \"{1}\"]", _containsRadioButton, answerText));
        }

        private void AwaitNextPage(bool expectQuestionPage = true) {
            var timeout = TimeSpan.FromSeconds(20);
            try {
                if (expectQuestionPage) {
                    new WebDriverWait(Driver, timeout).Until(ExpectedConditions.ElementExists(By.CssSelector(".multiple-choice--radio")));
                }
            } catch (WebDriverTimeoutException) {
                Assert.Fail(string.Format("The next question page didn't load in the awaited time ({0}s). Current page title is '{1}'.", timeout.Seconds, Header.Text));
            }
            new WebDriverWait(Driver, TimeSpan.FromSeconds(1));
        }

        private IEnumerable<string> GetAnswersText() {
            var answers = Driver.FindElements(By.CssSelector("[type='radio']")).ToList().Select(r => r.GetAttribute("value"));
            answers = answers.Select(a => {
                dynamic j = JObject.Parse(a);
                return "'" + (string) j.title.ToString() + "'";
            });
            return answers;
        }

        private void SelectAnswerBy(By by) {
            var availableAnswers = GetAnswersText();
            Assert.IsTrue(Driver.ElementExists(by), string.Format("Expected answer couldn't be found for question '{0}'. Tried to find answer {1}. Available answers were:\n{2}", Header.Text, by, string.Join("\n", availableAnswers)));
            Driver.FindElement(by).Click();
        }
    }
}