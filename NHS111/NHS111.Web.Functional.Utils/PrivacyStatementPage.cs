using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{


    public class PrivacyStatementPage : LayoutPage
    {
        private const string _privacyHeaderText = "Privacy statement";
        private const string _privacyParagraphText = "This privacy policy tells you what information we collect when you use the NHS 111 online service. It also tells you:";

        [FindsBy(How = How.CssSelector, Using = "main[id='content'] > div.privacy > h1")]
        public IWebElement PrivacyHeader { get; set; }

        [FindsBy(How=How.CssSelector, Using = "main[id='content'] > div > p:nth-of-type(1)")]
        public IWebElement PrivacyParagraph { get; set; }

        public PrivacyStatementPage(IWebDriver driver) : base(driver)
        {

        }

        public void Verify()
        {
            Assert.IsTrue(PrivacyHeader.Displayed);
            Assert.IsTrue(PrivacyParagraph.Displayed);

            Assert.AreEqual(_privacyHeaderText, PrivacyHeader.Text);
            Assert.AreEqual(_privacyParagraphText, PrivacyParagraph.Text);
        }
    }
}
