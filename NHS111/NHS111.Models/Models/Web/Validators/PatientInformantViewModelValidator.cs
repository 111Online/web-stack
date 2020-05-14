using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class PatientInformantViewModelValidator : AbstractValidator<PatientInformantViewModel> 
    {
        public PatientInformantViewModelValidator()
        {
            RuleFor(p => p.SelfName.Forename).NotEmpty().When(p => p.Informant == InformantType.Self); ;
            RuleFor(p => p.SelfName.Surname).NotEmpty().When(p => p.Informant == InformantType.Self);

            RuleFor(p => p.PatientName.Forename).NotEmpty().When(p => p.Informant == InformantType.ThirdParty);
            RuleFor(p => p.PatientName.Surname).NotEmpty().When(p => p.Informant == InformantType.ThirdParty);
            RuleFor(p => p.InformantName.Forename).NotEmpty().When(p => p.Informant == InformantType.ThirdParty);
            RuleFor(p => p.InformantName.Surname).NotEmpty().When(p => p.Informant == InformantType.ThirdParty);
        }

    }

    public class PersonViewModelValidatior : AbstractValidator<PersonViewModel>
    {
        public PersonViewModelValidatior()
        {
            RuleFor(p => p.Forename)
                .SetValidator(new PersonNameValidator<PersonViewModel, string>(p => p.Forename));
            RuleFor(p => p.Surname)
                .SetValidator(new PersonNameValidator<PersonViewModel, string>(p => p.Surname));
        }
    }
}
