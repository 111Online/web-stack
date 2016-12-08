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
        public string City { get; set; }
        public string County { get; set; }
        public string UPRN { get; set; }
    }

    [Validator(typeof(PersonalInfoAddressViewModelValidator))]
    public class PersonalInfoAddressViewModel : AddressInfoViewModel
    {  
    }

}