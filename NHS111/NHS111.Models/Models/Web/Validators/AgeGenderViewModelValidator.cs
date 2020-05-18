using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class AgeGenderViewModelValidator : AbstractValidator<AgeGenderViewModel>
    {
        public AgeGenderViewModelValidator()
        {
            RuleFor(p => p.Gender)
                .NotEmpty()
                .WithMessage("Please enter your sex");
            RuleFor(p => p.Age)
                .NotEmpty()
                .WithMessage("Please enter your age")
                .SetValidator(new AgeMinimumValidator<AgeGenderViewModel, int>(u => u.Age))
                .WithMessage("Sorry, this service is not available for children under 5 years of age, for medical advice please call 111.")
                .SetValidator(new AgeMaximumValidator<AgeGenderViewModel, int>(u => u.Age))
                .WithMessage("Please enter a valid age");
        }
    }
}
