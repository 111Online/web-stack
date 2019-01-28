﻿using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NHS111.Web.Functional.Utils
{
    public class FeedbackSection : LayoutPage
    {
        
        [FindsBy(How = How.ClassName, Using = "feedback")]
        internal IWebElement Feedback { get; set; }
        
        [FindsBy(How = How.ClassName, Using = "feedback__submit")]
        private IWebElement FeedbackSubmitButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "feedback__input")]
        private IWebElement FeedbackTextarea { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".feedback summary")]
        private IWebElement FeedbackSummary { get; set; }

        
        [FindsBy(How = How.CssSelector, Using = ".feedback #feedback-details")]
        private IWebElement FeedbackSuccessText { get; set; }

        public FeedbackSection(IWebDriver driver) : base(driver)
        {
            OpenDetails();
        }

        public void ClickSubmitButton()
        {
            FeedbackSubmitButton.Submit();
        }
        
        public void VerifyFeedbackDisplayed()
        {
            Assert.IsTrue(Feedback.Displayed);
        }

        public void OpenDetails()
        {
            FeedbackSummary.Click();
        }

        public void VerifyButtonDisabled()
        {
            Assert.IsFalse(FeedbackSubmitButton.Enabled);
        }

        public void VerifyButtonEnabled()
        {
            Assert.IsTrue(FeedbackSubmitButton.Enabled);
        }

        public void TypeInTextarea(string text)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("var feedback = document.getElementsByClassName('feedback__input')[0]; feedback.innerHTML='"+ text +"'; feedback.dispatchEvent(new Event('input', { bubbles: true }))");
        }

        public void VerifySuccessTextDisplayed()
        {
            // Requires the smallest of delays just because it shows as displayed = false until animation is complete
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(1));
            wait.Until(drv => FeedbackSuccessText.Displayed);
            Assert.IsTrue(FeedbackSuccessText.Displayed);
        }
    }
}
