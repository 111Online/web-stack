using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(PatientInformantViewModelValidator))]
    public class PatientInformantViewModel
    {
        public InformantType Informant { get; set; }
        public PersonViewModel InformantName { get; set; }
        public PersonViewModel PatientName { get; set; }
        public PersonViewModel SelfName { get; set; }
    }

    [Validator(typeof(PersonViewModelValidatior))]
    public class PersonViewModel
    {
        private string _forename;
        public string Forename { 
            get { return this._forename; }
            set { this._forename = !string.IsNullOrEmpty(value) ? value.Trim() : value; }
        }


        private string _surname;
        public string Surname
        {
            get { return this._surname; }
            set { this._surname = !string.IsNullOrEmpty(value) ? value.Trim() : value; }
        }
    }
    [Validator(typeof(EmailAddressViewModelValidator))]
    public class EmailAddressViewModel
    {
        private string _emailAddress = "";

        // If the skip link is used, this ensures email is set to empty string.
        public string EmailAddress { 
            get { return !Skipped ? _emailAddress : ""; }
            set { _emailAddress = !string.IsNullOrEmpty(value) ? value.Trim() : value; } 
        }

        public bool Skipped { get; set; }

        public bool Provided
        {
            get { return !string.IsNullOrWhiteSpace(EmailAddress); } 
        }

        public bool ProvidedOrSkipped {
            get { return Skipped || Provided; }
        }
    }

    public enum InformantType
    {
        NotSpecified,
        Self,
        ThirdParty
    }
}
