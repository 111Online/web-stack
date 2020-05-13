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
    class FromDateOfBirthViewModelToPersonalDetailsViewModel : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DateOfBirthViewModel, PersonalDetailViewModel>().ConvertUsing((ps, psd) =>
            {
                var dateOfBirthViewModel = ps.SourceValue as DateOfBirthViewModel;
                var personalDetails = dateOfBirthViewModel.PersonalDetailsViewModel;
                personalDetails.UserInfo.DoB = dateOfBirthViewModel.DoB;
                return personalDetails;
            });
        }
    }

}
