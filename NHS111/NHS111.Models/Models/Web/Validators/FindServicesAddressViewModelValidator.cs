using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class FindServicesAddressViewModelValidator : AbstractValidator<FindServicesAddressViewModel>
    {
        public FindServicesAddressViewModelValidator()
        {
            When(a => !a.IsPostcodeFirst || !string.IsNullOrEmpty(a.Postcode), () =>
            {
                RuleFor(p => p.Postcode)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .SetValidator(new PostCodeFormatValidator<FindServicesAddressViewModel, string>(u => u.Postcode))
                    .WithMessage("Please enter a valid UK postcode")
                    .SetValidator(new PostCodeAllowedValidator<FindServicesAddressViewModel, string>(u => u.Postcode))
                    .WithMessage("Sorry, this service is not currently available in your area.  Please call NHS 111 for advice now.");
            });
        }
    }
}
