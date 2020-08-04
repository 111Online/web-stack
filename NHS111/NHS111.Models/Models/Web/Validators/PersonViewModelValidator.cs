using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class PersonViewModelValidator : AbstractValidator<NHS111.Models.Models.Web.PersonalDetails.PersonViewModel>
    {
        public PersonViewModelValidator()
        {
            RuleFor(p => p.Forename).SetValidator(new FirstNameValidator<PersonalDetails.PersonViewModel, string>(p => p.Forename));

            RuleFor(p => p.Surname).SetValidator(new LastNameValidator<PersonalDetails.PersonViewModel, string>(p => p.Surname));
        }
    }
}
