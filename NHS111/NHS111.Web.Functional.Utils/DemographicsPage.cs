using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NHS111.Web.Functional.Utils
{
    public class DemographicsPage : LayoutPage
    {
        private const string _headerText = "Tell us about you, or the person you're asking about";
        private const string _sexValidationMessage = "Please enter your sex";
        private const string _ageEmptyValidationMessage = "Please enter your age";
        private const string _ageTooLowValidation = "Sorry, this service is not available for children under 5 years of age, for medical advice please call 111.";
        private const string _ageTooHighValidation = "Please enter a valid age";

        [FindsBy(How = How.CssSelector, Using = "h1")]
        private IWebElement Header { get; set; }

        [FindsBy(How = How.CssSelector, Using = "label[for='Male']")]
        private IWebElement MaleButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "label[for='Female']")]
        private IWebElement FemaleButton { get; set; }

        [FindsBy(How = How.Id, Using = "UserInfo_Demography_Age")]
        private IWebElement AgeInput { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span[data-valmsg-for='UserInfo.Demography.Age']")]
        private IWebElement AgeValidationMessageElement { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span[data-valmsg-for='UserInfo.Demography.Gender']")]
        private IWebElement SexValidationMessageElement { get; set; }

        [FindsBy(How = How.ClassName, Using = "button--next")]
        private IWebElement NextButton { get; set; }

        public DemographicsPage(IWebDriver driver) : base(driver)
        {
        }

        public void SelectSexAndAge(string sex, int age)
        {
            SelectSex(sex);
            SetAge(age);
        }

        public void SelectSex(string sex)
        {
            if (sex == TestScenerioSex.Male)
                MaleButton.Click();
            else
                FemaleButton.Click();
        }

        public void SetAge(int age)
        {
            AgeInput.SendKeys(age.ToString());
        }

        public void VerifyHeader()
        {
            Assert.IsTrue(Header.Displayed);
            Assert.AreEqual(_headerText, Header.Text);
        }

        public SearchPage NextPage()
        {
            NextButton.Submit();
            return new SearchPage(Driver);
        }

        public QuestionInfoPage NextPageDeeplink()
        {
            NextButton.Submit();
            return new QuestionInfoPage(Driver);
        }
        public void VerifyAgeInputBox(string sex, string age)
        {
            SelectSex(sex);
            AgeInput.SendKeys(age);
            NextButton.Submit();

            var searchPage = new SearchPage(Driver);
            searchPage.VerifyHeader();
        }

        public void VerifyNoSexValidation(int age)
        {
            SetAge(age);
            NextButton.Submit();

            Assert.IsTrue(SexValidationMessageElement.Displayed);
            Assert.AreEqual(_sexValidationMessage, SexValidationMessageElement.Text);
        }

        public void VerifyNoAgeValidation(string sex)
        {
            SelectSex(sex);
            NextButton.Submit();

            Assert.IsTrue(AgeValidationMessageElement.Displayed);
            Assert.AreEqual(_ageEmptyValidationMessage, AgeValidationMessageElement.Text);
        }

        public void VerifyTooOldAgeShowsValidation(string sex, int age)
        {
            SelectSex(sex);
            SetAge(age);
            NextButton.Submit();

            Assert.IsTrue(AgeValidationMessageElement.Displayed);
            Assert.AreEqual(_ageTooHighValidation, AgeValidationMessageElement.Text);
        }

        public void VerifyTooYoungAgeShowsValidation(string sex, int age)
        {
            SelectSex(sex);
            SetAge(age);
            NextButton.Submit();

            Assert.IsTrue(AgeValidationMessageElement.Displayed);
            Assert.AreEqual(_ageTooLowValidation, AgeValidationMessageElement.Text);
        }

        public void VerifyTabbingOrder(int age)
        {
            var maleButton = TabToFirstPageBodyElement();
            maleButton.SendKeys(Keys.Space);
            var ageInput = maleButton.Tab(Driver)
                .Tab(Driver);
            ageInput.SendKeys(age.ToString());
            
            var nextButton = ageInput.Tab(Driver);
            nextButton.SendKeys(Keys.Enter);

            var searchPage = new SearchPage(Driver);
            searchPage.VerifyHeader();
        }
    }
}
