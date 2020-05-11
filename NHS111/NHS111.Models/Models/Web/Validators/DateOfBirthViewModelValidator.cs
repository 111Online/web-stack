using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NHS111.Models.Models.Web.PersonalDetails;

namespace NHS111.Models.Models.Web.Validators
{
    public class DateOfBirthViewModelValidator : AbstractValidator<DateOfBirthViewModel>
    {
        public DateOfBirthViewModelValidator()
        {
            RuleFor(p => p.Day).SetValidator(new DateDayValidator<UserInfo, int?>(m => m.Day));
            RuleFor(p => p.Month).SetValidator(new DateMonthValidator<UserInfo, int?>(m => m.Month));
            RuleFor(p => p.Year).SetValidator(new DateYearValidator<UserInfo, int?>(m => m.Year));
            RuleFor(p => p.DoB).SetValidator(new DateOfBirthValidator<UserInfo, DateTime?>(m => m.DoB));
        }
    }
}
