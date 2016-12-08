using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class PersonalInfoAddressViewModelValidator : AbstractValidator<PersonalInfoAddressViewModel>
    {
        //private const string PostcodeRegex = @"^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))[0-9][A-Za-z]{2})$";

        public PersonalInfoAddressViewModelValidator()
        {
            //WORKS
            //RuleFor(p => p.Postcode).NotEmpty();

            //WORKS
            RuleFor(p => p.Postcode).NotEmpty().WithMessage("Please enter a valid UK postcode");

            RuleFor(p => p.Postcode)
                .Must(BeAValidPostcode)
                //.SetValidator(new PostCodeFormatValidator<PersonalInfoAddressViewModel, string>(u => u.Postcode))
                .WithMessage("Please enter a valid UK postcode");
            //.Cascade(CascadeMode.StopOnFirstFailure)
            //.SetValidator(new PostCodeFormatValidator<PersonalInfoAddressViewModel, string>(u => u.Postcode))
            //.WithMessage("Please enter a valid UK postcode");
            //.SetValidator(new PostCodeAllowedValidator<PersonalInfoAddressViewModel, string>(u => u.Postcode))
            //.WithMessage("Sorry, this service is not available outside of Leeds, for medical advice please call 111.");
            //RuleFor(a => a.AddressLine1).NotEmpty();
            //RuleFor(a => a.City).NotEmpty();
        }

        private static bool BeAValidPostcode(string postcode)
        {
            return false;
        }
    }
}
