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
            RuleFor(q => q.AnswerInputValue).NotEmpty().When(q => q.QuestionType == QuestionType.Integer).WithMessage("Please enter a number");
            RuleFor(q => q.AnswerInputValue).Cascade(CascadeMode.StopOnFirstFailure)
                .SetValidator(new IntegerAgeValidator())
                .When(q => q.QuestionType == QuestionType.Integer && q.Answers.First().Title.ToLower().Equals("age"))
                .WithMessage("Please enter a valid age");
            RuleFor(q => q.AnswerInputValue).Cascade(CascadeMode.StopOnFirstFailure)
                .SetValidator(new IntegerSymptomsStartedValidator())
                .When(q => q.QuestionType == QuestionType.Integer && q.Answers.First().Title.ToLower().Equals("symptomsstarted"))
                .WithMessage("Please enter a valid number");
            RuleFor(q => q.SelectedAnswer).NotEmpty().When(q => q.QuestionType == QuestionType.Boolean).WithMessage("Please select an answer");
            RuleFor(q => q.AnswerInputValue).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(s => s.ToCharArray().All(char.IsDigit))
                .Matches("^((07[0-9]{9,9})|((\\+|00)[1-9]{1,4})[0-9]{6,11})$")
                .When(q => q.QuestionType == QuestionType.Telephone).WithMessage("Please give a valid uk telephone number");
            RuleFor(q=> q.DateAnswer).SetValidator(new DateTimeInPastValidator()).When(q => q.QuestionType == QuestionType.Date).WithMessage("Please enter a valid date");
        }

        public class IntegerAgeValidator : IntegerValidator
        {
            public IntegerAgeValidator()
            {
                var i = 0;
                RuleFor(p => Convert.ToInt32(p))
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(120)
                    .WithMessage("Please enter a valid age");
            }
        }

        public class IntegerSymptomsStartedValidator : IntegerValidator
        {
            public IntegerSymptomsStartedValidator()
            {
                var i = 0;
                RuleFor(p => Convert.ToInt32(p))
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(10).WithMessage("Number of days can't be more than 10");
            }
        }

        public class IntegerValidator : AbstractValidator<string>
        {
            public IntegerValidator()
            {
                var i = 0;
                RuleFor(p => p)
                    .Must(p => int.TryParse(p, out i)).WithMessage("Please enter a number");
            }
        }
    }
}
