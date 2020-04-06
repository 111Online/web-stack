using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    class EmailAddressViewModelValidator : AbstractValidator<EmailAddressViewModel>
    {
        public EmailAddressViewModelValidator()
        {

            RuleFor(m => m)
                .NotEmpty().When(a => !a.Skipped)
                .SetValidator(new EmailAddressValidator<EmailAddressViewModel, string>(a => a.EmailAddress));
        }
    }
}
