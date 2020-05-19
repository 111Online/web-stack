using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.ITK;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeVIewModelToITKDispatchRequest : Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<OutcomeViewModel, ITKDispatchRequest>()
              .ForMember(dest => dest.Authentication, opt => opt.Ignore())
              .ForMember(dest => dest.PatientDetails, opt => opt.MapFrom(src => src))
              .ForMember(dest => dest.ServiceDetails, opt => opt.MapFrom(src => src))
              .ForMember(dest => dest.CaseDetails, opt => opt.MapFrom(src => src)); ;

            Mapper.CreateMap<OutcomeViewModel, CaseDetails>()
              .ConvertUsing<FromOutcomeViewModelToCaseDetailsConverter>();

            Mapper.CreateMap<PersonalDetailViewModel, PatientDetails>()
                .ConvertUsing<FromPersonalDetailViewModelToPatientDetailsConverter>();

            Mapper.CreateMap<OutcomeViewModel, ServiceDetails>()
                .ConvertUsing<FromOutcomeViewModelToServiceDetailsConverter>();
        }
    }
}
