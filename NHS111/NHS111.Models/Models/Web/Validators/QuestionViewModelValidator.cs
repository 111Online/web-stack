using FluentValidation;
using NHS111.Models.Models.Domain;
using System;
using System.Linq;

namespace NHS111.Models.Models.Web.Validators
{
    public class QuestionViewModelValidator : AbstractValidator<QuestionViewModel>
    {
        public QuestionViewModelValidator()
        {
            RuleFor(q => q.SelectedAnswer).NotEmpty().When(q => q.QuestionType == QuestionType.Choice).WithMessage("Please select an answer");
            RuleFor(q => q.AnswerInputValue).NotEmpty().When(q => q.QuestionType == QuestionType.String || q.QuestionType == QuestionType.Text).WithMessage("Please select an answer");
            RuleFor(q => q.AnswerInputValue).Must(BeANumber).When(q => q.QuestionType == QuestionType.Integer).WithMessage("Please enter a number");
            RuleFor(q => q.AnswerInputValue).Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeANumber)
                .SetValidator(new IntegerAgeValidator())
                .When(q => q.QuestionType == QuestionType.Integer && q.Answers.First().Title.ToLower().Equals("age"))
                .WithMessage("Please enter a valid age");
            RuleFor(q => q.AnswerInputValue).Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeANumber)
                .SetValidator(new IntegerSymptomsStartedValidator(10))
                .When(q => q.QuestionType == QuestionType.Integer && q.Answers.First().Title.ToLower().Equals("symptomsstarted"))
                .WithMessage("Please enter a valid number");
            RuleFor(q => q.SelectedAnswer).NotEmpty().When(q => q.QuestionType == QuestionType.Boolean).WithMessage("Please select an answer");
            RuleFor(q => q.AnswerInputValue).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(s => s.ToCharArray().All(char.IsDigit))
                .Matches("^((07[0-9]{9,9})|((\\+|00)[1-9]{1,4})[0-9]{6,11})$")
                .When(q => q.QuestionType == QuestionType.Telephone).WithMessage("Please give a valid uk telephone number");
            RuleFor(q => q.DateAnswer).SetValidator(new DateTimeInPastValidator()).When(q => q.QuestionType == QuestionType.Date).WithMessage("Please enter a valid date");
        }

        public class IntegerAgeValidator : AbstractValidator<string>
        {
            public IntegerAgeValidator()
            {
                RuleFor(p => Convert.ToInt32(p))
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(119)
                    .WithMessage("Please enter a valid age");
            }
        }

        private bool BeANumber(string value)
        {
            int i;
            return int.TryParse(value, out i);
        }

        public class IntegerSymptomsStartedValidator : AbstractValidator<string>
        {
            public IntegerSymptomsStartedValidator()
            {
                RuleFor(p => Convert.ToInt32(p))
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(10).WithMessage("Number of days can't be more than 10");
            }

            public IntegerSymptomsStartedValidator(int Limit)
            {
                var i = 0;

                RuleFor(p => Convert.ToInt32(p))
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(Limit).WithMessage("Number of days can't be more than {0}",Limit);
            }
        }

    }
}
