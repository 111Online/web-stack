using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    using System.Linq;

    public class UserInfoValidator : AbstractValidator<UserInfo>
    {
        public UserInfoValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty();
            RuleFor(p => p.LastName).NotEmpty();
            RuleFor(p => p.TelephoneNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Please provide a telephone number")
                .Must(s => s.ToCharArray().All(char.IsDigit))
                .WithMessage("Please enter only numbers")
                .Length(10, 13)
                .WithMessage("Please enter a number between 10 and 13 digits");
            RuleFor(p => p.Day).SetValidator(new DateOfBirthValidator<UserInfo, int?>(m => m.Day));
            RuleFor(p => p.Month).SetValidator(new DateOfBirthValidator<UserInfo, int?>(m => m.Month));
            RuleFor(p => p.Year).SetValidator(new DateOfBirthValidator<UserInfo, int?>(m => m.Year));
        }
    }
}
