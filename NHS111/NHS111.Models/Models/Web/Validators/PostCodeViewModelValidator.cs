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
            RuleFor(p => p.PostCode).SetValidator(new PostCodeFormatValidator<PostcodeViewModel, string>(u => u.PostCode));
            RuleFor(p => p.PostCode).SetValidator(new PostCodeAllowedValidator<PostcodeViewModel, string>(u => u.PostCode));
        }
    }
}
