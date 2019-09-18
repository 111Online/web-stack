using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{


    public class CookiesStatementPage : LayoutPage
    {
        private const string _cookiesHeaderText = "Cookies on 111.nhs.uk";
        private const string _cookiesParagraphText = "Cookies are small files saved on your phone, tablet or computer when you visit a website.";

        [FindsBy(How = How.CssSelector, Using = "main[id='content'] > div.cookies > h1")]
        public IWebElement CookiesHeader { get; set; }

        [FindsBy(How=How.CssSelector, Using = "main[id='content'] > div > p:nth-of-type(1)")]
        public IWebElement CookiesParagraph { get; set; }

        public CookiesStatementPage(IWebDriver driver) : base(driver)
        {

        }

        public void Verify()
        {
            Assert.IsTrue(CookiesHeader.Displayed);
            Assert.IsTrue(CookiesParagraph.Displayed);

            Assert.AreEqual(_cookiesHeaderText, CookiesHeader.Text);
            Assert.AreEqual(_cookiesParagraphText, CookiesParagraph.Text);
        }
    }
}
