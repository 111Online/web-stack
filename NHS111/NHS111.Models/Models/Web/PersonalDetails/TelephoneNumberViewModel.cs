using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web.PersonalDetails
{
    [Validator(typeof(TelephoneNumberValidator))]
    public class TelephoneNumberViewModel
    {
        public TelephoneNumberViewModel() { }
        public TelephoneNumberViewModel(PersonalDetailViewModel personalDetailViewModel)
        {
            PersonalDetailsViewModel = personalDetailViewModel;
        }
        public string TelephoneNumber { get; set; }
        public PersonalDetailViewModel PersonalDetailsViewModel { get; private set; }
    }
}
