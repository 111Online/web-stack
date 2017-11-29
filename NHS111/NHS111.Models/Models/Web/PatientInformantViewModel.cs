using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(PatientInformantViewModelValidator))]
    public class PatientInformantViewModel
    {
        public InformantType Informant { get; set; }
        public PersonViewModel InformantName { get; set; }
        public PersonViewModel PatientName { get; set; }
        public PersonViewModel SelfName { get; set; }
    }

    [Validator(typeof(PersonViewModelValidatior))]
    public class PersonViewModel
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
    }

    public enum InformantType
    {
        NotSpecified,
        Self,
        ThirdParty
    }
}
