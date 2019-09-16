using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Validators;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]
    public class UserInfoValidatorTests
    {
        private UserInfo PopulateUserInfoTelephone(string telephoneNumber)
        {
            return new UserInfo
            {
                TelephoneNumber = telephoneNumber,
                Day = 10,
                Month = 10,
                Year = 1980
            };
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_Home_returns_true()
        {
            var sut = new UserInfoValidator();
            Assert.IsTrue(sut.Validate(PopulateUserInfoTelephone("02270032002")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_Mobile_returns_true()
        {
            var sut = new UserInfoValidator();
            Assert.IsTrue(sut.Validate(PopulateUserInfoTelephone("07804555666")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_HomeWithSpaces_returns_true()
        {
            var sut = new UserInfoValidator();
            Assert.IsTrue(sut.Validate(PopulateUserInfoTelephone("0227 003 2002")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_TypeTalkWithSpaces_returns_true()
        {
            var sut = new UserInfoValidator();
            Assert.IsTrue(sut.Validate(PopulateUserInfoTelephone("18002 0227 003 2002")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_BeginsWithTwo_returns_false()
        {
            var sut = new UserInfoValidator();
            Assert.IsFalse(sut.Validate(PopulateUserInfoTelephone("22078 855886")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_BeginsWithPlusFourtyFour_returns_true()
        {
            var sut = new UserInfoValidator();
            Assert.IsTrue(sut.Validate(PopulateUserInfoTelephone("+447855 666555")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_HasPlusFourtyFourInMiddle_returns_false()
        {
            var sut = new UserInfoValidator();
            Assert.IsFalse(sut.Validate(PopulateUserInfoTelephone("07855 +44666555")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_BeginsWithPlusFourtyOne_USA_returns_false()
        {
            var sut = new UserInfoValidator();
            Assert.IsFalse(sut.Validate(PopulateUserInfoTelephone("+417855 666555")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_BeginsWithFourtyOne_returns_false()
        {
            var sut = new UserInfoValidator();
            Assert.IsFalse(sut.Validate(PopulateUserInfoTelephone("417855 666555")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_Brackets_returns_false()
        {
            var sut = new UserInfoValidator();
            Assert.IsFalse(sut.Validate(PopulateUserInfoTelephone("(020)78888777")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_ZeroInternationalPrefixUKWithZeroInMiddleOfNumber_returns_true()
        {
            var sut = new UserInfoValidator();
            Assert.IsTrue(sut.Validate(PopulateUserInfoTelephone("004478880077")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_ZeroInternationalPrefixUS_returns_false()
        {
            var sut = new UserInfoValidator();
            Assert.IsFalse(sut.Validate(PopulateUserInfoTelephone("004178888007")).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_Empty_returns_false()
        {
            var sut = new UserInfoValidator();
            Assert.IsFalse(sut.Validate(PopulateUserInfoTelephone(string.Empty)).IsValid);
        }

        [Test]
        public void UserInfoValidator_TelephoneNumber_WithHyphen_returns_false()
        {
            var sut = new UserInfoValidator();
            Assert.IsFalse(sut.Validate(PopulateUserInfoTelephone("0207-00101")).IsValid);
        }
    }
}
