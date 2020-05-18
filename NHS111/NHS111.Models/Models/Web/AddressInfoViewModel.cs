using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;


namespace NHS111.Models.Models.Web
{
    public class AddressInfoViewModel
    {
        public string Postcode { get; set; }
        public string HouseNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Thoroughfare { get; set; }
        public string Ward { get; set; }
        private string _city;
        public string City
        {
            get
            {
                if (_city == null) return _city;
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                return textInfo.ToTitleCase(_city.ToLower());
            }
            set { _city = value; }
        }
        public string County { get; set; }
        public string UDPRN { get; set; }
        public bool IsPostcodeFirst { get; set; }
        public bool IsInPilotArea { get; set; }

        public string FormattedAddress
        {
            get
            {
                var addressString = "";

                if (!String.IsNullOrWhiteSpace(AddressLine1)) addressString = AddressLine1;
                if (!String.IsNullOrWhiteSpace(AddressLine2)) addressString += (", " + AddressLine2);
                if (!String.IsNullOrWhiteSpace(AddressLine3)) addressString += (", " + AddressLine3);
                if (!string.IsNullOrEmpty(City)) addressString += (", " + City);
                if (!string.IsNullOrEmpty(County) && !City.ToLower().Equals(County.ToLower())) addressString += (", " + County);
                if (!string.IsNullOrEmpty(Postcode)) addressString += (", " + Postcode);

                return addressString;
            }
        }

        public string FormattedPostcode
        {
            get
            {
                if (Postcode == null) return null;
                var normalisedPostcode = Postcode.Trim().Replace(" ", "").ToUpper();
                if (normalisedPostcode.Length < 4) return normalisedPostcode;
                return normalisedPostcode.Insert(normalisedPostcode.Length - 3, " ");
            }
        }
    }

    public class PersonalInfoAddressViewModel : AddressInfoViewModel
    {
    }

    public class FindServicesAddressViewModel : AddressInfoViewModel
    {
    }

    [Validator(typeof(PersonalInfoCurrentAddressViewModelValidator))]
    public class CurrentAddressViewModel : PersonalDetailsAddressViewModel
    {

    }

    [Validator(typeof(HomeAddressModelValidator))]
    public class PersonalDetailsAddressViewModel : AddressInfoViewModel
    {
        public List<SelectListItem> AddressPicker { get; set; }
        public string SelectedAddressFromPicker { get; set; }
        public string PreviouslyEnteredPostcode { get; set; }

        public PersonalDetailsAddressViewModel()
        {
            AddressPicker = new List<SelectListItem>();
        }
    }

    public class AddressInfoCollectionViewModel
    {
        public IEnumerable<AddressInfoViewModel> Addresses { get; set; }
        public PostcodeValidatorResponse ValidatedPostcodeResponse { get; set; }

        public static AddressInfoCollectionViewModel InvalidSyntaxResponse = new AddressInfoCollectionViewModel { ValidatedPostcodeResponse = PostcodeValidatorResponse.InvalidSyntax, Addresses = new List<AddressInfoViewModel>() };
    }
}