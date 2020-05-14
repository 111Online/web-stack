using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web.PersonalDetails
{
    [Validator(typeof(InformantTypeValidator))]
    public class InformantTypeViewModel
    {
        public InformantTypeViewModel()
        {
        }

        public InformantTypeViewModel(PersonalDetailViewModel personalDetailViewModel)
        {
            PersonalDetailViewModel = personalDetailViewModel;
        }
        public InformantType Informant { get; set; }
        public PersonalDetailViewModel PersonalDetailViewModel { get; set; }
    }
}
