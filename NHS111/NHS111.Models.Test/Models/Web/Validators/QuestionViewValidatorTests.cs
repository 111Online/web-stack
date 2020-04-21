using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Validators;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]
    public class QuestionViewValidatorTests
    {
        QuestionViewModelValidator _validator;

        [SetUp]
        public void Init()
        {
            _validator = new QuestionViewModelValidator();
        }
        private QuestionViewModel PopulateQuestionViewPhoneNumber(string telephoneNumber)
        {
            return new QuestionViewModel
            {
                AnswerInputValue = telephoneNumber,
                QuestionType = QuestionType.Telephone
            };
        }

        [Test]
        public void Invalid_Phone_Number_With_Text()
        {
            notValidPhoneNumber("This is not a number");
        }

        [Test]
        public void Mixed_number_and_text_rejected()
        {
            notValidPhoneNumber("07abcdefghi");
            notValidPhoneNumber("+447abcdefghi");
            notValidPhoneNumber("00447abcdefghi");
        }

        [Test]
        public void Text_prefix_rejected()
        {
            notValidPhoneNumber("X07777777777");
            notValidPhoneNumber("X+447077777777");
            notValidPhoneNumber("X00447077777777");
        }

        [Test]
        public void Uk_Mobile_Number_is_valid()
        {
            validPhoneNumber("07777777777");
            validPhoneNumber("+447077777777");
            validPhoneNumber("00447077777777");
        }

        [Test]
        public void Valid_uk_landline_number_is_not_valid()
        {
            notValidPhoneNumber("02077777777");
            notValidPhoneNumber("+442077777777");
            notValidPhoneNumber("00442077777777");
        }

        [Test]
        public void Whitespace_is_removed()
        {
            validPhoneNumber("07 777 777 777");
            validPhoneNumber("+44707777 77 77");
            validPhoneNumber("00 4470 7777 7777");
        }

        [Test]
        public void UK_Invalid_Characters_in_Mobile_Number_with_Country_Code()
        {
            notValidPhoneNumber("+4470d77777777");
            notValidPhoneNumber("004470d77777777");
        }

        [Test]
        public void Uk_Mobile_Too_Long()
        {
            notValidPhoneNumber("0720812121212121212121212");
            notValidPhoneNumber("+44720812121212121212121212");
            notValidPhoneNumber("0044720812121212121212121212");
        }

        [Test]
        public void International_Number_Longer_than_15_digits()
        {
            notValidPhoneNumber("+1234567890123456");
            notValidPhoneNumber("001234567890123456");
        }

        [Test]
        public void Valid_International_Number()
        {
            validPhoneNumber("+123456789012345");
            validPhoneNumber("00123456789012345");
        }

        private void notValidPhoneNumber(string number)
        {
            Assert.IsFalse(_validator.Validate(PopulateQuestionViewPhoneNumber(number)).IsValid);
        }

        private void validPhoneNumber(string number)
        {
            Assert.IsTrue(_validator.Validate(PopulateQuestionViewPhoneNumber(number)).IsValid);
        }
    }
}
