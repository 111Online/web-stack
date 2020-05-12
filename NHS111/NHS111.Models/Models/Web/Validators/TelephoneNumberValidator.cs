using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NHS111.Models.Models.Web.PersonalDetails;

namespace NHS111.Models.Models.Web.Validators
{
    public class TelephoneNumberValidator : AbstractValidator<TelephoneNumberViewModel>
    {
        public TelephoneNumberValidator()
        {
            RuleFor(m => m.TelephoneNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(s => s.ToCharArray().All(char.IsDigit))
                .Must(s => s.StartsWith("0") || s.StartsWith("1"))
                .Matches("^[0-1,\\s, +]{1}[0-9,\\s]{8,20}$")
                .Length(9, 21);
        }
    }
}
