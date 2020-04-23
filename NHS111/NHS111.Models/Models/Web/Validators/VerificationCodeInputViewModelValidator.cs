using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    class VerificationCodeInputViewModelValidator : AbstractValidator<VerificationCodeInputViewModel>
    {
        public VerificationCodeInputViewModelValidator()
        {
            RuleFor(m => m)
                .SetValidator(new SMSVerificationCodeValidator<VerificationCodeInputViewModel, string>(a => a.InputValue));
        }
    }
}
