using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.PersonalDetails;
using NHS111.Models.Models.Web.Validators;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]
    public class TelephoneNumberValidatorTests
    { 
        private TelephoneNumberViewModel PopulateTelephoneViewModel(string telephoneNumber)
        {
            return new TelephoneNumberViewModel
            {
                TelephoneNumber = telephoneNumber,
            };
        }

        [TestCase("02270032002", true, Category = "PhoneValidationIsValid", Description = "TelephoneNumber_Home_returns_true")]
        [TestCase("07804555666", true, Category = "PhoneValidationIsValid", Description = "TelephoneNumber_Mobile_returns_true")]
        [TestCase("0227 003 2002", true, Category = "PhoneValidationIsValid", Description = "TelephoneNumber_HomeWithSpaces_returns_true")]
        [TestCase("18002 0227 003 2002", true, Category = "PhoneValidationIsValid", Description = "TelephoneNumber_TypeTalkWithSpaces_returns_true")]
        [TestCase("22078 855886", false, Category = "PhoneValidationIsInvalid", Description = "TelephoneNumber_BeginsWithTwo_returns_false")]
        [TestCase("+447855 666555", true, Category = "PhoneValidationIsValid", Description = "TelephoneNumber_BeginsWithPlusFourtyFour_returns_true")]
        [TestCase("07855 +44666555", false, Category = "PhoneValidationIsInvalid", Description = "TelephoneNumber_HasPlusFourtyFourInMiddle_returns_false")]
        [TestCase("+417855 666555", false, Category = "PhoneValidationIsInvalid", Description = "TelephoneNumber_BeginsWithPlusFourtyOne_USA_returns_false")]
        [TestCase("417855 666555", false, Category = "PhoneValidationIsInvalid", Description = "TelephoneNumber_BeginsWithFourtyOne_returns_false")]
        [TestCase("(020)78888777", false, Category = "PhoneValidationIsInvalid", Description = "TelephoneNumber_Brackets_returns_false")]
        [TestCase("004178888007", false, Category = "PhoneValidationIsInvalid", Description = "TelephoneNumber_ZeroInternationalPrefixUS_returns_false")]
        [TestCase("", false, Category = "PhoneValidationIsInvalid", Description = "TelephoneNumber_Empty_returns_false")]
        [TestCase("0207-00101", false, Category = "PhoneValidationIsInvalid", Description = "TelephoneNumber_WithHyphen_returns_false")]
        [TestCase("004478880077", true, Category = "PhoneValidationIsValid", Description = "TelephoneNumber_ZeroInternationalPrefixUKWithZeroInMiddleOfNumber_returns_true")]
        public void TelephoneNumberValidator_validate(string telephoneNumber, bool shouldBeValid)
        {
            var sut = new TelephoneNumberValidator();
            if(shouldBeValid)
                Assert.IsTrue(sut.Validate(PopulateTelephoneViewModel(telephoneNumber)).IsValid);
            else
            {
                Assert.IsFalse(sut.Validate(PopulateTelephoneViewModel(telephoneNumber)).IsValid);
            }
        }
    }
}
