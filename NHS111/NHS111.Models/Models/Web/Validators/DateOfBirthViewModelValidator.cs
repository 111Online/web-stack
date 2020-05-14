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
            RuleFor(p => p.Day).SetValidator(new DateDayValidator<DateOfBirthViewModel, int?>(m => m.Day));
            RuleFor(p => p.Month).SetValidator(new DateMonthValidator<DateOfBirthViewModel, int?>(m => m.Month));
            RuleFor(p => p.Year).SetValidator(new DateYearValidator<DateOfBirthViewModel, int?>(m => m.Year));
            RuleFor(p => p.DoB).SetValidator(new DateOfBirthValidator<DateOfBirthViewModel, DateTime?>(m => m.DoB));
        }
    }
}
