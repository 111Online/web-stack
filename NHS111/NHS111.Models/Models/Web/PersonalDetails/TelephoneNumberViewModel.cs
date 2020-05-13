
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
            if (personalDetailViewModel.UserInfo != null &&
                !string.IsNullOrEmpty(personalDetailViewModel.UserInfo.TelephoneNumber))
                _telephoneNumber = personalDetailViewModel.UserInfo.TelephoneNumber;
        }

        private string _telephoneNumber;
        public string TelephoneNumber {
            get
            {
                if (string.IsNullOrEmpty(_telephoneNumber)) return string.Empty;

                _telephoneNumber = _telephoneNumber.Replace(" ", "");
                _telephoneNumber = RemoveValidInternationalPrefix(_telephoneNumber);

                return _telephoneNumber;
            }

            set { _telephoneNumber = value; }
        }
        private string RemoveValidInternationalPrefix(string telephoneNumber)
        {
            if (telephoneNumber.StartsWith("00"))
                telephoneNumber = "+" + telephoneNumber.Substring(2);

            if (telephoneNumber.StartsWith("+44"))
                telephoneNumber = "0" + telephoneNumber.Substring(3);

            return telephoneNumber;
        }

        public PersonalDetailViewModel PersonalDetailsViewModel { get;  set; }
    }
}
