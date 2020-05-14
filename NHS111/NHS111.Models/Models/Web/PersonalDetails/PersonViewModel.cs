
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web.PersonalDetails
{

    [Validator(typeof(PersonViewModelValidatior))]
    public class PersonViewModel
    {
        public PersonViewModel()
        {
        }

        public PersonViewModel(PersonalDetailViewModel personalDetailViewModel)
        {
            PersonalDetailViewModel = personalDetailViewModel;
        }
        private string _forename;
        public string Forename
        {
            get { return this._forename; }
            set { this._forename = !string.IsNullOrEmpty(value) ? value.Trim() : value; }
        }


        private string _surname;
        public string Surname
        {
            get { return this._surname; }
            set { this._surname = !string.IsNullOrEmpty(value) ? value.Trim() : value; }
        }

        public PersonalDetailViewModel PersonalDetailViewModel { get; set; }
    }
}
