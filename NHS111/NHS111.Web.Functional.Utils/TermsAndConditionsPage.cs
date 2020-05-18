using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{


    public class TermsAndConditionsPage : LayoutPage
    {
        private const string _termsHeaderText = "Terms and conditions";
        private const string _termsParagraphText = "This website is operated by NHS Digital. These terms of use govern your use of this website (https://111.nhs.uk). We may amend these terms of use from time to time, and the revised version will be effective when displayed here.";

        [FindsBy(How = How.CssSelector, Using = "main[id='content'] > div.terms > h1")]
        public IWebElement TermsHeader { get; set; }

        [FindsBy(How = How.CssSelector, Using = "main[id='content'] > div > ul:nth-of-type(1) > li:nth-of-type(1)")]
        public IWebElement TermsParagraph { get; set; }

        public TermsAndConditionsPage(IWebDriver driver) : base(driver)
        {

        }

        public void Verify()
        {
            Assert.IsTrue(TermsHeader.Displayed);
            Assert.IsTrue(TermsParagraph.Displayed);

            Assert.AreEqual(_termsHeaderText, TermsHeader.Text);
            Assert.AreEqual(_termsParagraphText, TermsParagraph.Text);
        }
    }
}
