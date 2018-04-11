using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class PersonalInfoAddressViewModelValidator : AbstractValidator<PersonalDetailsAddressViewModel>
    {
        public PersonalInfoAddressViewModelValidator()
        {
            RuleFor(a => a.Postcode).NotEmpty();
            RuleFor(a => a.AddressLine1).NotEmpty();
            RuleFor(a => a.City).NotEmpty();
            RuleFor(a => a.SelectedAddressFromPicker).NotEmpty().When(a => 
                   string.IsNullOrWhiteSpace(a.AddressLine1) 
                || string.IsNullOrWhiteSpace(a.City)
                || string.IsNullOrWhiteSpace(a.Postcode));
        }
    }
}
