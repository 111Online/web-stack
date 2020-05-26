using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{


    public class BritishSignLanguagePage : LayoutPage
    {
        private const string _anotherLanguageHeaderText = "If you need help in another language";
        private const string _signLanguageText = "British Sign Language for deaf users";

        [FindsBy(How = How.ClassName, Using = "nhsuk-heading-xl")]
        public IWebElement anotherLanguageHeader { get; set; }

        [FindsBy(How = How.ClassName, Using = "nhsuk-heading-l")]
        public IWebElement signLanguageHeader { get; set; }

        public BritishSignLanguagePage(IWebDriver driver) : base(driver)
        {

        }

        public void Verify()
        {
            Assert.IsTrue(anotherLanguageHeader.Displayed);
            Assert.IsTrue(signLanguageHeader.Displayed);

            Assert.AreEqual(_anotherLanguageHeaderText, anotherLanguageHeader.Text);
            Assert.AreEqual(_signLanguageText, signLanguageHeader.Text);
        }
    }
}
