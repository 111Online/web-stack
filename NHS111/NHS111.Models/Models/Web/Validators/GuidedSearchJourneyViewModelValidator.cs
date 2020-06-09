using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class GuidedSearchJourneyViewModelValidator : AbstractValidator<GuidedSearchJourneyViewModel>
    {
        public GuidedSearchJourneyViewModelValidator()
        {
            RuleFor(g => g.PathwayNo).NotEmpty().WithMessage("Choose one option");
        }
    }
}
