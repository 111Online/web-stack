using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;
using System;


namespace NHS111.Models.Models.Web
{
    [Validator(typeof(UserInfoValidator))]
    public class UserInfo
    {
        public AgeGenderViewModel Demography { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    
        public DateTime? DoB { get; set; }

        public string TelephoneNumber
        {
            get;
            set;
        }

        private String RemoveValidInternationalPrefix(string telephoneNumber)
        {
            if (telephoneNumber.StartsWith("00"))
                telephoneNumber = "+" + telephoneNumber.Substring(2);

            if (telephoneNumber.StartsWith("+44"))
                telephoneNumber = "0" + telephoneNumber.Substring(3);

            return telephoneNumber;
        }

        public string EmailAddress { get; set; }

        public AddressInfoViewModel HomeAddress { get; set; }
        public FindServicesAddressViewModel CurrentAddress { get; set; }

        public UserInfo()
        {
            HomeAddress = new AddressInfoViewModel();
            CurrentAddress = new FindServicesAddressViewModel();
        }
    }
}