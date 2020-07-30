using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class PatientInformantViewModelValidator : AbstractValidator<PatientInformantViewModel>
    {
        public PatientInformantViewModelValidator()
        {
            RuleFor(p => p.InformantName.Forename).SetValidator(new FirstNameValidator<PatientInformantViewModel, string>(p => p.InformantName.Forename));

            RuleFor(p => p.InformantName.Surname).SetValidator(new LastNameValidator<PatientInformantViewModel, string>(p => p.InformantName.Surname));
        }
    }
}
