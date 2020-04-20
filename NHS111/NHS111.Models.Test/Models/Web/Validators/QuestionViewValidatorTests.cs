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
            Assert.IsFalse(_validator.Validate(PopulateQuestionViewPhoneNumber("This is not a number")).IsValid);
        }

        [Test]
        public void Invalid_Phone_Number_starting_with_number()
        {
            Assert.IsFalse(_validator.Validate(PopulateQuestionViewPhoneNumber("07abcdefghi")).IsValid);
        }

        [Test]
        public void Valid_Uk_Mobile_Number()
        {
            Assert.IsTrue(_validator.Validate(PopulateQuestionViewPhoneNumber("07777777777")).IsValid);
        }

        [Test]
        public void Valid_uk_landline_number_returns_false()
        {
            Assert.IsFalse(_validator.Validate(PopulateQuestionViewPhoneNumber("02077777777")).IsValid);
        }

    }
}
