using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class SearchJourneyViewModelValidator : AbstractValidator<SearchJourneyViewModel>
    {
        public SearchJourneyViewModelValidator()
        {
            RuleFor(p => p.SanitisedSearchTerm)
                .NotEmpty()
                .WithMessage("Please enter the symptom you're concerned about");
        }
    }
}
