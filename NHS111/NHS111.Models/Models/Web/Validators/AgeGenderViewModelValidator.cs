using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class AgeGenderViewModelValidator : AbstractValidator<AgeGenderViewModel>
    {
        public AgeGenderViewModelValidator()
        {
            RuleFor(p => p.Gender)
                .NotEmpty();
            RuleFor(p => p.Age)
                .SetValidator(new AgeValidator<AgeGenderViewModel, int>(u => u.Age))
                .WithMessage(
                    "Sorry, this service is not available for children under 5 years of age, for medical advice please call 111.")
                .LessThan(201).WithMessage("The age you entered is incorrect")
                .GreaterThanOrEqualTo(0).WithMessage("The age you entered is incorrect");
        }
    }
}
