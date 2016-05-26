using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Web.Presentation.Models;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeViewModelToDosCase : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<OutcomeViewModel, DosCase>()
            //    .ForMember(dest => dest.PostCode,
            //        opt => opt.ResolveUsing<PostcodeResolver>().FromMember(src => src.UserInfo))
            //    .ForMember(dest => dest.Disposition,
            //        opt => opt.ResolveUsing<DispositionResolver>().FromMember(src => src.Id))
            //    .ForMember(dest => dest.SymptomDiscriminatorList,
            //        opt => opt.ResolveUsing<SymptomDiscriminatorListResolver>().FromMember(dest => dest.SymptomDiscriminator))
            //    .ForMember(dest => dest.Gender, 
            //        opt => opt.ResolveUsing<GenderResolver>().FromMember(src => src.UserInfo.Gender))
            //    .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.UserInfo.Age))
            //    .ForMember(dest => dest.Surgery, opt => opt.MapFrom(src => src.SurgeryViewModel.SelectedSurgery));  
        }

        
    }
}
