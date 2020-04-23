using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Validators;
using NUnit.Framework;
using System.Linq;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]
    public class PostCodeFormatValidatorTests
    {
        [Test]
        public void Is_valid_null_string_returns_false()
        {
            Assert.IsFalse(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode(null));
        }

        [Test]
        public void Is_valid_empty_string_returns_false()
        {
            Assert.IsFalse(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode(string.Empty));
        }

        [Test]
        public void Is_valid_only_number_string_returns_false()
        {
            Assert.IsFalse(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode("1234"));
        }

        [Test]
        public void Is_valid_only_letters_string_returns_false()
        {
            Assert.IsFalse(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode("abcdef"));
        }

        [Test]
        public void Is_valid_only_non_alpha_numeric_string_returns_false()
        {
            Assert.IsFalse(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode("£$%^^&^"));
        }

        [Test]
        public void Is_valid_contains_non_alpha_numeric_string_returns_false()
        {
            Assert.IsFalse(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode("ab $%^ n6$^ ef"));
        }

        [Test]
        public void Is_valid_postcode_string_returns_true()
        {
            Assert.IsTrue(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode("SO30 2UN"));
        }

        [Test]
        public void Is_valid_postcode_case_insensitive_returns_true()
        {
            Assert.IsTrue(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode("So30 2Un"));
        }

        [Test]
        public void Is_valid_postcode_without_space_returns_true()
        {
            Assert.IsTrue(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode("So302Un"));
        }

        [Test]
        public void Is_valid_postcode_with_trailing_spaces_returns_true()
        {
            Assert.IsTrue(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode("So30 2Un "));
        }

        [Test]
        public void Is_valid_postcode_with_leading_spaces_returns_true()
        {
            Assert.IsTrue(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode(" So30 2Un"));
        }

        [Test]
        public void Is_valid_postcode_with_spaces_either_side_returns_true()
        {
            Assert.IsTrue(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode(" So30 2Un "));
        }

        [Test]
        public void Is_valid_partial_postcode_returns_false()
        {
            Assert.IsFalse(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode("So30"));
        }

        [Test]
        public void Is_valid_all_return_true()
        {
            //postcodes used in testing
            var postcodes = new[] { "SO30 2UN", "LS17 7NZ", "SO30 9UH", "OX99 2ZA", "m1 3ed", "W1p 0aa" };
            var sut = postcodes.Select(PostCodeFormatValidator<PersonalInfoAddressViewModel, string>.IsAValidPostcode);
            Assert.IsTrue(sut.All(p => p));
        }
    }
}
