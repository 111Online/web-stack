using System;
using FluentValidation;
using FluentValidation.Attributes;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(AddressInfoViewModelValidator))]
    public class AddressInfoViewModel
    {
        public string PostCode { get; set; }
        public string HouseNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string UPRN { get; set; }
    }

    public class AddressInfoViewModelValidator : AbstractValidator<AddressInfoViewModel>
    {
        public AddressInfoViewModelValidator()
        {
            RuleFor(a => a.PostCode).NotEmpty();
            RuleFor(a => a.AddressLine1).NotEmpty();
            RuleFor(a => a.City).NotEmpty();
        }


    }

}