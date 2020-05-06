using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.PersonalDetails;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromTelephoneNumberViewModelToPersonalDetailsViewModel : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<TelephoneNumberViewModel, PersonalDetailViewModel>().ConvertUsing((ps, psd) =>
            {
                var telephoneNumberViewModel = ps.SourceValue as TelephoneNumberViewModel;
                var personalDetails = telephoneNumberViewModel.PersonalDetailsViewModel;
                personalDetails.UserInfo.TelephoneNumber = telephoneNumberViewModel.TelephoneNumber;
                return personalDetails;
            });
        }
    }
}
