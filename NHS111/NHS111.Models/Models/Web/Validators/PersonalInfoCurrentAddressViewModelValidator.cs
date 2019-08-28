using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class PersonalInfoCurrentAddressViewModelValidator : AbstractValidator<CurrentAddressViewModel>
    {
        public PersonalInfoCurrentAddressViewModelValidator()
        { 
            RuleFor(a => a.Postcode).NotEmpty();
            RuleFor(a => a.AddressLine1).NotEmpty();
            RuleFor(a => a.City).NotEmpty();
        }
    }
}
