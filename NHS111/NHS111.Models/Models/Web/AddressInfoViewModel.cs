using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;
using RestSharp.Extensions;


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
                var firstPart = String.IsNullOrWhiteSpace(this.HouseNumber) ? "" : this.HouseNumber;
                var secondPart = String.IsNullOrWhiteSpace(this.Thoroughfare) ? this.Ward : this.Thoroughfare;

                var addressString = (!String.IsNullOrEmpty(firstPart) ? firstPart + " " : "") + secondPart;
                if (!String.IsNullOrWhiteSpace(AddressLine1) && addressString.ToLower() != AddressLine1.ToLower())
                    addressString = AddressLine1 + ", " + addressString;

                if (!string.IsNullOrEmpty(this.AddressLine2)) addressString += (", " + this.AddressLine2);
                if (!string.IsNullOrEmpty(this.AddressLine3)) addressString += (", " + this.AddressLine3);
                if (!string.IsNullOrEmpty(this.City)) addressString += (", " + this.City);
                if (!string.IsNullOrEmpty(this.County)) addressString += (", " + this.County);
                if (!string.IsNullOrEmpty(this.Postcode)) addressString += (", " + this.Postcode);

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