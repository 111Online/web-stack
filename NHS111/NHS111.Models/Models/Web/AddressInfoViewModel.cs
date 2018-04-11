using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

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
        public string City { get; set; }
        public string County { get; set; }
        public string UPRN { get; set; }
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

    [Validator(typeof(PersonalInfoAddressViewModelValidator))]
    public class CurrentAddressViewModel : PersonalDetailsAddressViewModel
    {
        
    }

    [Validator(typeof(HomeAddressModelValidatior))]
    public class PersonalDetailsAddressViewModel : AddressInfoViewModel
    {
        public List<SelectListItem> AddressPicker { get; set; }
        public string SelectedAddressFromPicker { get; set; }
        public string PreviouslyEnteredPostcode { get; set; }
        public string AddressOptions { get; set; }

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