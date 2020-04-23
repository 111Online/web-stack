using FluentValidation;
using FluentValidation.Attributes;
using System;

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
                .Must(s => s.StartsWith("0") || s.StartsWith("1"))
                .Matches("^[0-1,\\s, +]{1}[0-9,\\s]{8,20}$")
                .Length(9, 21);
            RuleFor(p => p.Day).SetValidator(new DateDayValidator<UserInfo, int?>(m => m.Day));
            RuleFor(p => p.Month).SetValidator(new DateMonthValidator<UserInfo, int?>(m => m.Month));
            RuleFor(p => p.Year).SetValidator(new DateYearValidator<UserInfo, int?>(m => m.Year));
            RuleFor(p => p.DoB).SetValidator(new DateOfBirthValidator<UserInfo, DateTime?>(m => m.DoB));
        }
    }


    public class DateTimeInPastValidator : DateTimeValidator
    {
        public DateTimeInPastValidator() : base()
        {
            RuleFor(p => p.Date.HasValue).Equal(true).WithMessage("Enter a valid date");
            RuleFor(p => p.Date).LessThanOrEqualTo(DateTime.Now).WithMessage("The date must not be in the future");
        }
    }


    public class DateTimeValidator : AbstractValidator<DateTimeViewModel>
    {
        public DateTimeValidator()
        {
            RuleFor(p => p.Day).SetValidator(new DateDayValidator<DateTimeViewModel, int?>(m => m.Day));
            RuleFor(p => p.Month).SetValidator(new DateMonthValidator<DateTimeViewModel, int?>(m => m.Month));
            RuleFor(p => p.Year).SetValidator(new DateYearValidator<DateTimeViewModel, int?>(m => m.Year));
            RuleFor(p => p.Date).SetValidator(new DateValidator<DateTimeViewModel, DateTime?>(m => m.Date));
        }
    }

    [Validator(typeof(DateTimeValidator))]
    public class DateTimeViewModel
    {
        public static DateTimeViewModel Build(string day, string month, string year)
        {
            int dayInt;
            int monthInt;
            int yearInt;
            int.TryParse(day, out dayInt);
            int.TryParse(month, out monthInt);
            int.TryParse(year, out yearInt);
            return new DateTimeViewModel() { Day = dayInt, Month = monthInt, Year = yearInt };
        }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        private DateTime? _date;
        public DateTime? Date
        {
            get
            {
                if (Year != null && Month != null && Day != null)
                {
                    try
                    {
                        _date = new DateTime(Year.Value, Month.Value, Day.Value);
                        return _date;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return null;
                    }
                }
                return null;
            }
        }



    }
}
