using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class FindServicesAddressViewModelValidator : AbstractValidator<FindServicesAddressViewModel>
    {
        public FindServicesAddressViewModelValidator()
        {
            RuleFor(p => p.Postcode)
                .SetValidator(new PostCodeFormatValidator<FindServicesAddressViewModel, string>(u => u.Postcode))
                .WithMessage("Please enter a valid UK postcode");
        }
    }
}
