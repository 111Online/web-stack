using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    public class InformantViewModel
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public bool IsInformantForPatient
        {
            get { return InformantType.Equals(InformantType.ThirdParty); }
        }

        public InformantType InformantType { get; set; }
    }
}
