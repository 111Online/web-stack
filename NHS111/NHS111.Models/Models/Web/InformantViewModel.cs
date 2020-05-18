using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(InformantViewModelValidator))]
    public class InformantViewModel
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public bool IsInformantForPatient { get; set; }
    }
}
