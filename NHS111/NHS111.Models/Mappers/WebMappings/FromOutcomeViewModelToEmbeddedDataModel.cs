using System;
using AutoMapper;
using NHS111.Models.Models.Business.MicroSurvey;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeViewModelToEmbeddedDataModel : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OutcomeViewModel, EmbeddedData>()
                .ForMember(dest => dest.JourneyId, opt => opt.MapFrom(src => src.JourneyId))
                .ForMember(dest => dest.DxCode, opt => opt.MapFrom(src => src.SurveyLink.DispositionCode))
                .ForMember(dest => dest.DispositionDate,
                    opt => opt.MapFrom(src =>
                        DateTime.SpecifyKind(src.SurveyLink.DispositionDateTime, DateTimeKind.Utc).Date.ToString("O")))
                .ForMember(dest => dest.DispositionTime,
                    opt => opt.MapFrom(src => src.SurveyLink.DispositionDateTime.TimeOfDay))
                .ForMember(dest => dest.Ccg, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.LaunchPage,
                    opt => opt.MapFrom(src =>
                        src.OutcomePage == OutcomePage.Outcome ? OutcomePage.Outcome.ToString() : ""))
                .ForMember(dest => dest.ValidationCallbackOfferd,
                    opt => opt.MapFrom(src => src.SurveyLink.ValidationCallbackOffered))
                .ForMember(dest => dest.ServicesOffered,
                    opt => opt.MapFrom(src => src.SurveyLink.ServiceOptions.Split(',')))
                .ForMember(dest => dest.ServiceCount, opt => opt.MapFrom(src => src.SurveyLink.ServiceCount))
                .ForMember(dest => dest.RecommendedServiceDosType,
                    opt => opt.MapFrom(src => src.RecommendedService.OnlineDOSServiceType.Id))
                .ForMember(dest => dest.RecommendedServiceId, opt => opt.MapFrom(src => src.RecommendedService.Id))
                .ForMember(dest => dest.ResommendedServiceName, opt => opt.MapFrom(src => src.RecommendedService.Name))
                .ForMember(dest => dest.RecommendedServiceAlias,
                    opt => opt.MapFrom(src => src.RecommendedService.PublicName))
                .ForMember(dest => dest.RecommendedServiceDistance,
                    opt => opt.MapFrom(src => src.RecommendedService.Distance))
                .ForMember(dest => dest.SdCode, opt => opt.MapFrom(src => src.SymptomDiscriminatorCode ?? ""))
                .ForMember(dest => dest.SdDescription,
                    opt => opt.MapFrom(src => src.SymptomDiscriminator.Description ?? ""))
                .ForMember(dest => dest.SgCode, opt => opt.MapFrom(src => src.SymptomGroup ?? ""));
            //.ForMember(dest => dest.SgDescription, opt => opt.Ignore());
        }
    }
}