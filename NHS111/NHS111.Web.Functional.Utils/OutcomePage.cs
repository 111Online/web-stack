using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class OutcomePage : DispositionPage<OutcomePage>
    {
        public const string BookCallBackText = "Book a call with a 111 nurse now";
        public const string Call111Text = "You need to call 111 to speak to an adviser now";

        [FindsBy(How = How.Id, Using = "FindService_CurrentPostcode")]
        private IWebElement PostcodeField { get; set; }

        //[FindsBy(How = How.Id, Using = "availableServices")]
        //private IWebElement DOSGroups { get; set; }

        [FindsBy(How = How.ClassName, Using = "cards")]
        private IWebElement DosResults { get; set; }


        [FindsBy(How = How.ClassName, Using = "summary")]
        private IList<IWebElement> DOSGroups { get; set; }


        public OutcomePage(IWebDriver driver) : base(driver)
        {
        }

        public void VerifyPageContainsDOSResults()
        {
            Assert.IsTrue(DOSGroups.Count() > 0, "No DoS result groupings found on page.");
            Assert.IsTrue(DosResults.Displayed);
            var results = DosResults.FindElements(By.ClassName("card"));

            Assert.IsTrue(results.Count > 0, "No DoS results found on page.");
        }

        public void VerifyDOSResultGroupExists(string groupText)
        {
            Assert.IsTrue(DOSGroups.Any(g => g.Text == groupText));
        }

        public override OutcomePage EnterPostCodeAndSubmit(string postcode)
        {
            PostcodeField.Clear();
            PostcodeField.SendKeys(postcode);
            PostcodeSubmitButton.Click();
            return new OutcomePage(Driver);
        }
    }
}
