using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    public class LocationInfoViewModel
    {
        public CurrentAddressViewModel PatientCurrentAddress { get; set; }
        public PersonalDetailsAddressViewModel PatientHomeAddress { get; set; }
        public HomeAddressSameAsCurrentWrapper HomeAddressSameAsCurrentWrapper { get; set; }

        public ChangePostcodeViewModel ChangePostcode { get; set; }

        public LocationInfoViewModel()
        {
            PatientCurrentAddress = new CurrentAddressViewModel();
            PatientHomeAddress = new PersonalDetailsAddressViewModel();
        }
    }

    [Validator(typeof(HomeAddressSameAsCurrentValidator))]
    public class HomeAddressSameAsCurrentWrapper
    {
        public HomeAddressSameAsCurrent? HomeAddressSameAsCurrent { get; set; }
    }

    public enum HomeAddressSameAsCurrent
    {
        Yes,
        No,
        DontKnow
    }
}
