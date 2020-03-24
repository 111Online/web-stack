using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Validators;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    class EmailAddressViewModelValidator : AbstractValidator<EmailAddressViewModel>
    {
        public EmailAddressViewModelValidator()
        {

            RuleFor(m => m)
                .NotEmpty().When(a => !a.Skipped)
                .SetValidator(new EmailAddressValidator<EmailAddressViewModel, string>(a => a.EmailAddress));
        }
    }
}
