using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.SmokeTest.Utils
{
    public class OutcomePage : DispositionPage<OutcomePage>
    {
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
            Assert.IsTrue(DOSGroups.Count() > 0);
            Assert.IsTrue(DosResults.Displayed);
            var results = DosResults.FindElements(By.ClassName("card"));

            Assert.IsTrue(results.Count > 0);
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
