using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class EnterVerificationCodeSMSPage : LayoutPage
    {
        [FindsBy(How = How.Id, Using = "smsVerificationCode")]
        public IWebElement VerificationCodeInput { get; set; }

        [FindsBy(How = How.Id, Using = "nextScreen")]
        public IWebElement NextButton { get; set; }

        [FindsBy(How = How.ClassName, Using = "nhsuk-heading-l")]
        public IWebElement HeadingElement { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"submitDetailsForm\"]/div/span")]
        public IWebElement InValidCodeValidationError { get; set; }

        public EnterVerificationCodeSMSPage(IWebDriver driver) : base(driver)
        {
        }

        public QuestionPage InputVerificationCodeAndSubmit(string verificationCode)
        {
            VerificationCodeInput.SendKeys(verificationCode);
            NextButton.Click();
            return new QuestionPage(Driver);
        }

        public void VerifyPageContent()
        {
            Assert.AreEqual("Enter your security code", HeadingElement.Text);
            Assert.True(VerificationCodeInput.Displayed);

            // Test validation
            NextButton.Click();
            Assert.AreEqual("Enter a valid number", InValidCodeValidationError.Text);
            Assert.True(InValidCodeValidationError.Displayed);
        }

    }
}
