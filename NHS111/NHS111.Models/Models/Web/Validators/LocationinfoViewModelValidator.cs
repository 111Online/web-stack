using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web.Validators
{
    public class LocationInfoViewModelValidator : AbstractValidator<LocationInfoViewModel>
    {
        public LocationInfoViewModelValidator()
        {
            RuleFor(m => m.HomeAddressSameAsCurrent).SetValidator(new HomeAddressSameAsCurrentValidator<LocationInfoViewModel, HomeAddressSameAsCurrent?>(m => m.HomeAddressSameAsCurrent));
            RuleFor(m => m.PatientHomeAddress).SetValidator(new PersonalInfoAddressViewModelValidator()).When(m =>
                m.HomeAddressSameAsCurrent.HasValue &&
                m.HomeAddressSameAsCurrent.Value == HomeAddressSameAsCurrent.No);
        }
    }


    public class HomeAddressModelValidator : AbstractValidator<PersonalDetailsAddressViewModel>
    {
        public HomeAddressModelValidator()
        {
            RuleFor(m => m.AddressLine1)
                .SetValidator(new HomeAddressValidator<PersonalDetailsAddressViewModel, string> (a=> a.AddressLine1));
            RuleFor(m => m.City)
                .SetValidator(new HomeAddressValidator<PersonalDetailsAddressViewModel, string>(a => a.City));
            RuleFor(m => m.Postcode)
                .SetValidator(new HomeAddressValidator<PersonalDetailsAddressViewModel, string>(a => a.Postcode));
            RuleFor(m => m.SelectedAddressFromPicker)
                .SetValidator(new HomeAddressValidator<PersonalDetailsAddressViewModel, string>(a => a.SelectedAddressFromPicker));


        }
    }
}
