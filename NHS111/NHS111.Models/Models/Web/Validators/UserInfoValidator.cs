using System;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    using System.Linq;

    public class UserInfoValidator : AbstractValidator<UserInfo>
    {
        public UserInfoValidator()
        {
            //RuleFor(p => p.FirstName).NotEmpty();
           // RuleFor(p => p.LastName).NotEmpty();
            RuleFor(p => p.TelephoneNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(s => s.ToCharArray().All(char.IsDigit))
                .Length(9, 15);
            RuleFor(p => p.Day).SetValidator(new DateOfBirthDayValidator<UserInfo, int?>(m => m.Day));
            RuleFor(p => p.Month).SetValidator(new DateOfBirthMonthValidator<UserInfo, int?>(m => m.Month));
            RuleFor(p => p.Year).SetValidator(new DateOfBirthYearValidator<UserInfo, int?>(m => m.Year));
            RuleFor(p => p.DoB).SetValidator(new DateOfBirthValidator<UserInfo, DateTime?>(m => m.DoB));
        }
    }
}
