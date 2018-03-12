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
    }
}