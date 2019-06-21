using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class OutcomePage : DispositionPage<OutcomePage> {

        public const string Cat2999Text = "Phone 999 now for an ambulance";
        public const string Cat3999Text = "Phone 999 now for an ambulance";
        public const string Cat4999Text = "Phone 999 for an ambulance";
        public const string ValidationCallbackText = "A nurse needs to phone you";
        public const string BookCallBackText = "Book a call with a 111 nurse now";
        public const string GetCallBackText = "Get a phone call from a nurse";
        public const string Call111Text = "You need to call 111 to speak to an adviser now";

        [FindsBy(How = How.Id, Using = "FindService_CurrentPostcode")]
        private IWebElement PostcodeField { get; set; }

        [FindsBy(How = How.Id, Using = "PreviousQuestionFromOutcome")]
        private IWebElement PreviousAnswer { get; set; }

        [FindsBy(How = How.CssSelector, Using = "div.survey-banner > p > a")]
        public IWebElement SurveyLink { get; set; }

        public OutcomePage(IWebDriver driver) : base(driver)
        {
            // Opens all DOS groupings
            ((IJavaScriptExecutor)driver).ExecuteScript("[...document.querySelectorAll('#availableServices details')].map((val) => val.setAttribute('open', true))");
        }

        public override OutcomePage EnterPostCodeAndSubmit(string postcode)
        {
            PostcodeField.Clear();
            PostcodeField.SendKeys(postcode);
            PostcodeSubmitButton.Click();
            return new OutcomePage(Driver);
        }

        public void VerifyIsCallbackAcceptancePage() {
            VerifyOutcome("A nurse needs to phone you", "Get a phone call from a nurse");
        }

        public void VerifyIsPersonalDetailsPage() {
            VerifyOutcome("Enter details");
        }
        public QuestionPage ClickPrevious()
        {
            PreviousAnswer.Click();
            return new QuestionPage(Driver);
        }

        public OutcomePage CompareAndVerify(string uniqueId)
        {
           return base.CompareAndVerify(this, uniqueId);
        }
    }
}
