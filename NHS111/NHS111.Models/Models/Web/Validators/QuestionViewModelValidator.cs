using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Web.Validators
{
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator()
        {
            RuleFor(q => q.SelectedAnswer).NotEmpty().When(q => q.QuestionType == QuestionType.Choice).WithMessage("Please select an answer");
            RuleFor(q => q.AnswerInputValue).NotEmpty().When(q => q.QuestionType == QuestionType.String || q.QuestionType == QuestionType.Text).WithMessage("Please select an answer");
        }
    }
}
