using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class SMSRegistrationTests : BaseTests
    {
        [Test]
        public void SMSRegistrationRegressionTest()
        {
            var covid19SMSRegistrationUrl = BaseUrl + "covid-19/sms";
            var mobileNumber = "07425474731";
            var verificationCodeInput = "654321";
            var age = 15;
            var daysSinceSymptomsStarted = 5;
            var livesAlone = true;

            var getTextMessagesPage = TestScenerios.LaunchCovid19SmsRegistration(Driver, covid19SMSRegistrationUrl);
            getTextMessagesPage.VerifyPageContent();

            var questionPage = getTextMessagesPage.NextPage();
            var verifySmsPage =  questionPage.AnswerSMSPhoneNumberAndSubmit(mobileNumber);
            verifySmsPage.VerifyPageContent();

            var enterVerificationCodeSmsPage = verifySmsPage.Submit();
            enterVerificationCodeSmsPage.VerifyPageContent();
            questionPage = enterVerificationCodeSmsPage.InputVerificationCodeAndSubmit(verificationCodeInput);

            questionPage = questionPage.EnterSMSAgeAndSubmit(age);
            questionPage = questionPage.EnterDaysSinceSymptomsStartedAndSubmit(daysSinceSymptomsStarted);
            var registrationPage = questionPage.AnswerDoYouLiveAloneAndSubmit(livesAlone);
            
            registrationPage.VerifyPageContent(mobileNumber, age.ToString(), daysSinceSymptomsStarted.ToString() + " days ago", 
                livesAlone ? "Yes" : "No");
        }
    }
}
