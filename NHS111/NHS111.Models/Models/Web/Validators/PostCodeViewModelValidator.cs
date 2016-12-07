using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class PostCodeViewModelValidator : AbstractValidator<PostcodeViewModel>
    {
        public PostCodeViewModelValidator()
        {
            RuleFor(p => p.Postcode).SetValidator(new PostCodeFormatValidator<PostcodeViewModel, string>(u => u.Postcode))
                .WithMessage("Please enter a valid UK postcode");
            RuleFor(p => p.Postcode).SetValidator(new PostCodeAllowedValidator<PostcodeViewModel, string>(u => u.Postcode))
                .WithMessage("Sorry, this service is not available outside of Leeds, for medical advice please call 111.");
        }
    }
}
