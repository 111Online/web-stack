using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class InformantViewModelValidator : AbstractValidator<InformantViewModel>
    {
        public InformantViewModelValidator()
        {
            RuleFor(i => i.Forename).SetValidator(new InformantForenameValidator<InformantViewModel, string>(m => m.Forename));
            RuleFor(i => i.Surname).SetValidator(new InformantSurnameValidator<InformantViewModel, string>(m => m.Surname));
        }
    }
}
