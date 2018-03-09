using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class LocationInfoViewModelValidator : AbstractValidator<LocationInfoViewModel>
    {
        public LocationInfoViewModelValidator()
        {
            RuleFor(m => m.HomeAddressSameAsCurrent).SetValidator(new HomeAddressSameAsCurrentValidator<LocationInfoViewModel, HomeAddressSameAsCurrent?>(m => m.HomeAddressSameAsCurrent));
            RuleFor(m => m.PatientHomeAddreess).SetValidator(new PersonalInfoAddressViewModelValidator()).When(m =>
                m.HomeAddressSameAsCurrent.HasValue &&
                m.HomeAddressSameAsCurrent.Value == HomeAddressSameAsCurrent.No);
        }
    }
}
