using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class SendSMSRegistrationPage : LayoutPage
    {
        public SendSMSRegistrationPage(IWebDriver driver) : base(driver)
        {
        }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/div[1]/dl/div[1]/dt")]
        public IWebElement MobileNumberTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/div[1]/dl/div[2]/dt")]
        public IWebElement AgeTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/div[1]/dl/div[3]/dt")]
        public IWebElement WhenSymptomsStartedTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/div[1]/dl/div[4]/dt")]
        public IWebElement DoYouLiveAloneTitle { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/div[1]/dl/div[1]/dd")]
        public IWebElement MobileNumberValue { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/div[1]/dl/div[2]/dd")]
        public IWebElement AgeValue { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/div[1]/dl/div[3]/dd")]
        public IWebElement WhenSymptomsStartedValue { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/div[1]/dl/div[4]/dd")]
        public IWebElement DoYouLiveAloneValue { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"content\"]/div[1]/p/a")]
        public IWebElement PrivacyPolicyLink { get; set; }

        [FindsBy(How = How.ClassName, Using = "nhsuk-heading-l")]
        public IWebElement HeadingElement { get; set; }

        public void VerifyPageContent(string expectedMobileNumber, string expectedAge, string expectedSymptomsStarted,
            string expectedDoYouLiveAlone)
        {
            // Verify values
            Assert.AreEqual(expectedMobileNumber, MobileNumberValue.Text);
            Assert.AreEqual(expectedAge, AgeValue.Text);
            Assert.AreEqual(expectedSymptomsStarted, WhenSymptomsStartedValue.Text);
            Assert.AreEqual(expectedDoYouLiveAlone, DoYouLiveAloneValue.Text);

            // Verify content
            Assert.AreEqual("Check your details", HeadingElement.Text);
            Assert.AreEqual("Mobile number", MobileNumberTitle.Text);
            Assert.AreEqual("Age", AgeTitle.Text);
            Assert.AreEqual("When symptoms started", WhenSymptomsStartedTitle.Text);
            Assert.AreEqual("Do you live alone", DoYouLiveAloneTitle.Text);
        }
    }
}
