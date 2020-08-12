using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace NHS111.Models.Models.Web.Validators
{
    public class LocationViewModelValidator : AbstractValidator<LocationViewModel>
    {
        public LocationViewModelValidator()
        {
            RuleFor(p => p.CurrentPostcode)
                .NotEmpty()
                .WithMessage("Enter a valid postcode");
        }
    }
}
