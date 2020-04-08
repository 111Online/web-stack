using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DataCapture;
using NUnit.Framework;
using System;

namespace NHS111.Models.Test.Mappers.WebMappings
{
    [TestFixture()]
    public class DataCaptureApiMapperTests
    {
        private SendSmsOutcomeViewModel _fromViewModel;

        [SetUp()]
        public void InitializeDataCaptureApiModelMapper()
        {
            _fromViewModel = new SendSmsOutcomeViewModel { VerificationCodeInput = new VerificationCodeInputViewModel() };
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.DataCaptureApiRequestMappings>());
        }

        [Test()]
        public void From_SendSmsOutcomeViewModel_To_GenerateSMSVerifyCodeRequestConverter_Configuration_IsValid()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test()]
        public void From_SendSmsOutcomeViewModel_To_VerifyCodeRequestConverter_Configuration_IsValid()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test()]
        public void From_SendSmsOutcomeViewModel_To_SubmitSMSRegistrationRequestConverter_Configuration_IsValid()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test()]
        public void From_SendSmsOutcomeViewModel_To_GenerateSMSVerifyCodeRequest_Converter_with_empty_string()
        {
            _fromViewModel.MobileNumber = string.Empty;

            var actualNumber = Mapper.Map<SendSmsOutcomeViewModel, GenerateSMSVerifyCodeRequest>(_fromViewModel).MobilePhoneNumber;

            Assert.AreEqual(string.Empty, actualNumber);
        }

        [Test()]
        public void From_SendSmsOutcomeViewModel_To_GenerateSMSVerifyCodeRequest_Converter_with_actual_number()
        {
            _fromViewModel.MobileNumber = "07999999999";

            var actualNumber = Mapper.Map<SendSmsOutcomeViewModel, GenerateSMSVerifyCodeRequest>(_fromViewModel).MobilePhoneNumber;

            Assert.AreEqual(_fromViewModel.MobileNumber, actualNumber);
        }

        [Test()]
        public void From_SendSmsOutcomeViewModel_To_VerifySMSCodeRequest_Converter_with_empty_string_for_number_and_verification_code()
        {
            _fromViewModel.MobileNumber = string.Empty;
            _fromViewModel.VerificationCodeInput.InputValue = string.Empty;

            var result = Mapper.Map<SendSmsOutcomeViewModel, VerifySMSCodeRequest>(_fromViewModel);

            Assert.AreEqual(string.Empty, result.MobilePhoneNumber);
        }

        [Test()]
        public void From_SendSmsOutcomeViewModel_To_VerifySMSCodeRequest_Converter_with_actual_number_and_empty_verification_code()
        {
            _fromViewModel.MobileNumber = "07999999999";
            _fromViewModel.VerificationCodeInput.InputValue = string.Empty;

            var result = Mapper.Map<SendSmsOutcomeViewModel, VerifySMSCodeRequest>(_fromViewModel);

            Assert.AreEqual("07999999999", result.MobilePhoneNumber);
            Assert.AreEqual(string.Empty, result.VerificationCodeInput);
        }

        [Test()]
        public void From_SendSmsOutcomeViewModel_To_VerifySMSCodeRequest_Converter_with_actual_number_and_actual_verification_code()
        {
            _fromViewModel.MobileNumber = "07999999999";
            _fromViewModel.VerificationCodeInput.InputValue = "123456";

            var result = Mapper.Map<SendSmsOutcomeViewModel, VerifySMSCodeRequest>(_fromViewModel);

            Assert.AreEqual("07999999999", result.MobilePhoneNumber);
            Assert.AreEqual("123456", result.VerificationCodeInput);
        }

        [Test]
        public void From_SendSmsOutcomeViewModel_To_SubmitSMSRegistrationRequest_Converter_with_actual_model()
        {
            _fromViewModel.JourneyId = Guid.Empty;
            _fromViewModel.CurrentPostcode = "hmmm!";
            _fromViewModel.Age = -1;
            _fromViewModel.MobileNumber = "123456789012";
            _fromViewModel.SymptomsStartedDaysAgo = -1;
            _fromViewModel.LivesAlone = true;
            _fromViewModel.VerificationCodeInput.InputValue = "123456";

            var result = Mapper.Map<SendSmsOutcomeViewModel, SubmitSMSRegistrationRequest>(_fromViewModel);

            Assert.AreEqual(Guid.Empty.ToString(), result.JourneyId);
            Assert.AreEqual("hmmm!", result.PostCode);
            Assert.AreEqual(-1, result.Age);
            Assert.AreEqual("123456789012", result.Phone);
            Assert.AreEqual(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), result.SymptomsStarted);
            Assert.IsTrue(result.LiveAlone);
            Assert.AreEqual("123456", result.VerificationCodeInput);
        }
    }
}
