
namespace NHS111.SmokeTest.Utils {
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;

    public class GatewayPage
        : HomePage {
        [FindsBy(How = How.Id, Using = "Postcode")]
        private IWebElement PostcodeTextBox { get; set; }

        [FindsBy(How = How.ClassName, Using = "button")]
        private IWebElement NextButton { get; set; }

        public GatewayPage(IWebDriver driver)
            : base(driver) {
            _headerText = "How 111 online works";
        }

        public bool IsEnabled { get { return Header.Displayed; } }

        public ModuleZeroPage SubmitPostcode(string postcode) {

            PostcodeTextBox.Clear();
            PostcodeTextBox.SendKeys(postcode);
            NextButton.Click();
            //todo need an await?
            return new ModuleZeroPage(Driver);
        }
    }
}
