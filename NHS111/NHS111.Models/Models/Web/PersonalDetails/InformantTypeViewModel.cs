using System;
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

        public bool InformantChanged
        {
            get
            {
                return Informant != InformantType.NotSpecified
                       && !PersonalDetailsViewModel.Informant.InformantType.Equals(Informant);
            }
        }

        public void ClearInformantDetails()
        {
            PersonalDetailsViewModel.Informant.Forename = string.Empty;
            PersonalDetailsViewModel.Informant.Surname = string.Empty;
            PersonalDetailsViewModel.UserInfo.FirstName = string.Empty;
            PersonalDetailsViewModel.UserInfo.LastName = string.Empty;
            PersonalDetailsViewModel.UserInfo.DoB = null;
        }

        public PersonalDetailViewModel PersonalDetailsViewModel { get; set; }
    }
}
