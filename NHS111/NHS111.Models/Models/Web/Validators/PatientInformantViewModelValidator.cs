using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class PersonViewModelValidatior : AbstractValidator<NHS111.Models.Models.Web.PersonalDetails.PersonViewModel>
    {
        public PersonViewModelValidatior()
        {
            RuleFor(p => p.Forename).NotEmpty();

            RuleFor(p => p.Surname).NotEmpty();
        }
    }
}
