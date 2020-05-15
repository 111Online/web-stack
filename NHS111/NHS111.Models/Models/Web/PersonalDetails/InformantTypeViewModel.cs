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
            PersonalDetailsViewModel = personalDetailViewModel;
            Informant = personalDetailViewModel.Informant.InformantType;
        }
        public InformantType Informant { get; set; }
        public PersonalDetailViewModel PersonalDetailsViewModel { get; set; }
    }
}
