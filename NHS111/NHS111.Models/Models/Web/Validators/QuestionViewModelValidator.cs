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
            RuleFor(q => q.AnswerInputValue).NotEmpty().When(q => q.QuestionType == QuestionType.Integer).WithMessage("Please give a number");
            RuleFor(q => q.SelectedAnswer).NotEmpty().When(q => q.QuestionType == QuestionType.Boolean).WithMessage("Please select an answer");
            RuleFor(q => q.AnswerInputValue).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(s => s.ToCharArray().All(char.IsDigit))
                .Matches("^((07[0-9]{9,9})|((\\+|00)[1-9]{1,4})[0-9]{6,11})$")
                .When(q => q.QuestionType == QuestionType.Telephone).WithMessage("Please give a valid uk telephone number");
            RuleFor(q=> q.DateAnswer).SetValidator(new DateTimeValidator()).When(q => q.QuestionType == QuestionType.Date).WithMessage("Please select a valid date");
        }
    }
}
