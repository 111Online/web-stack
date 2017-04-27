using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Web.Validators
{
    public class AgeGenderViewModelValidator : AbstractValidator<AgeGenderViewModel>
    {
        public AgeGenderViewModelValidator()
        {
            RuleFor(p => p.Gender).Cascade(CascadeMode.Continue)
                .NotNull();
            RuleFor(p => p.Age)
                .SetValidator(new AgeValidator<AgeGenderViewModel, int>(u => u.Age));
        }
    }
}
