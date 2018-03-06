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

            RuleFor(m => m.PatientCurrentAddress.AddressLine1).NotEmpty();
            RuleFor(a => a.PatientCurrentAddress.Postcode).NotEmpty();
            RuleFor(a => a.PatientCurrentAddress.AddressLine1).NotEmpty();
            RuleFor(a => a.PatientCurrentAddress.City).NotEmpty();
            RuleFor(a => a.PatientCurrentAddress.PreviouslyEnteredPostcode).NotEmpty();
            RuleFor(m => m.PatientCurrentAddress.SelectedAddressFromPicker).NotEmpty().When(a =>
                   string.IsNullOrWhiteSpace(a.PatientCurrentAddress.AddressLine1)
                || string.IsNullOrWhiteSpace(a.PatientCurrentAddress.City)
                || string.IsNullOrWhiteSpace(a.PatientCurrentAddress.Postcode)); ;
           
            RuleFor(m => m.HomeAddressSameAsCurrent).SetValidator(new HomeAddressSameAsCurrentValidator<LocationInfoViewModel, bool?>(m => m.HomeAddressSameAsCurrent));

        }
    }
}
