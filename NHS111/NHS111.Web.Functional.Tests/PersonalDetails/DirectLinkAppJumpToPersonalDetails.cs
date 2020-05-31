using OpenQA.Selenium;
using System.Collections.Generic;

namespace NHS111.Web.Functional.Tests.PersonalDetails
{
    public class DirectLinkAppJumpToPersonalDetails
    {
        private readonly IWebDriver _driver;
        private Dictionary<string, string> _values = new Dictionary<string, string>();

        public DirectLinkAppJumpToPersonalDetails(IWebDriver driver)
        {
            _driver = driver;
        }

        public DirectLinkAppJumpToPersonalDetails SetOutcomeGroupId(string outcomeGroupId)
        {
            _values.Add("OutcomeGroup.Id", outcomeGroupId);
            return this;
        }

        public DirectLinkAppJumpToPersonalDetails SetPostcode(string postcode)
        {
            _values.Add("CurrentPostcode", postcode);
            return this;
        }

        public DirectLinkAppJumpToPersonalDetails SetPostValue(string fieldName, string fieldValue)
        {
            _values.Add(fieldName, fieldValue);
            return this;
        }

        public void Navigate(string appBaseUrl)
        {
            OpenDirectLinkApp();
            SetBaseUrl(appBaseUrl);
            ChangePostData();
            SubmitDataToNavigate();
        }

        private void OpenDirectLinkApp()
        {
            _driver.Url = "https://directlinkapp.z33.web.core.windows.net/bookACall";
            _driver.Navigate();
            _driver.FindElement(By.CssSelector("#target")).Click();
        }

        private void SetBaseUrl(string appBaseUrl)
        {
            IWebElement baseUrl = _driver.FindElement(By.CssSelector("#baseUrl"));
            baseUrl.Clear();
            baseUrl.SendKeys(appBaseUrl);
        }

        private void ChangePostData()
        {
            foreach (var item in _values)
            {
                IWebElement inputField = _driver.FindElement(By.CssSelector("[name='" + item.Key + "']"));
                inputField.Clear();
                inputField.SendKeys(item.Value);
            }
        }

        private void SubmitDataToNavigate()
        {
            _driver.FindElement(By.CssSelector("#submit")).Click();
        }
    }
}
