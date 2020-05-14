using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web.PersonalDetails
{
    [Validator(typeof(DateOfBirthViewModelValidator))]
    public class DateOfBirthViewModel
    {
        public DateOfBirthViewModel() { }
        public DateOfBirthViewModel(PersonalDetailViewModel personalDetailViewModel)
        {
            PersonalDetailsViewModel = personalDetailViewModel;
            if (personalDetailViewModel.UserInfo != null &&
                personalDetailViewModel.UserInfo.DoB.HasValue)
            {
                _dob = personalDetailViewModel.UserInfo.DoB;
                Day = personalDetailViewModel.UserInfo.DoB.Value.Day;
                Month = personalDetailViewModel.UserInfo.DoB.Value.Month;
                Year = personalDetailViewModel.UserInfo.DoB.Value.Year;
            }
        }

        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        private DateTime? _dob;
        public DateTime? DoB
        {
            get
            {
                if (Year != null && Month != null && Day != null)
                {
                    try
                    {
                        _dob = new DateTime(Year.Value, Month.Value, Day.Value);
                        return _dob;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return null;
                    }
                }
                return null;
            }
        }

        public PersonalDetailViewModel PersonalDetailsViewModel { get; set; }
    }
}
