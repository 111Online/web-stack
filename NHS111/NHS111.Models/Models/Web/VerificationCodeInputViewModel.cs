using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(VerificationCodeInputViewModelValidator))]
    public class VerificationCodeInputViewModel
    {
        public string InputValue { get; set; }
        public bool IsCorrect { get; set; }
    }
}
