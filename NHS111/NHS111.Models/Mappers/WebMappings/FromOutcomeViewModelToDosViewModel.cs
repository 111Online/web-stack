using AutoMapper;
using NHS111.Models.Models.Web;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeViewModelToDosViewModel : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OutcomeViewModel, DosViewModel>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.UserInfo.Age))
                .ForMember(dest => dest.CareAdvices, opt => opt.MapFrom(src => src.CareAdvices))
                .ForMember(dest => dest.CheckCapacitySummaryResultList, opt => opt.MapFrom(src => src.CheckCapacitySummaryResultList))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.UserInfo.Gender))
                .ForMember(dest => dest.CareAdviceMarkers, opt => opt.MapFrom(src => src.CareAdviceMarkers))
                .ForMember(dest => dest.CareAdvices, opt => opt.MapFrom(src => src.CareAdvices))
                .ForMember(dest => dest.CaseId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.JourneyJson, opt => opt.MapFrom(src => src.JourneyJson))
                .ForMember(dest => dest.PathwayNo, opt => opt.MapFrom(src => src.PathwayNo))
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.UserInfo.CurrentAddress.PostCode)
                        ? src.UserInfo.CurrentAddress.PostCode
                        : src.UserInfo.HomeAddress.PostCode))
                .ForMember(dest => dest.SelectedServiceId, opt => opt.MapFrom(src => src.SelectedServiceId))
                .ForMember(dest => dest.Surgery, opt => opt.MapFrom(src => src.SurgeryViewModel.SelectedSurgery))
                .ForMember(dest => dest.SymptomDiscriminator, opt => opt.MapFrom(src => src.SymptomDiscriminator))
                .ForMember(dest => dest.SymptomGroup, opt => opt.MapFrom(src => src.SymptomGroup))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

        }
    }
}