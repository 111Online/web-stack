using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class PersonViewModelValidatior : AbstractValidator<NHS111.Models.Models.Web.PersonalDetails.PersonViewModel>
    {
        public PersonViewModelValidatior()
        {
            RuleFor(p => p.Forename).NotEmpty();

            RuleFor(p => p.Surname).NotEmpty();
        }
    }
}
