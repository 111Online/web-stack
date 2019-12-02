using System.Linq;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Models.Models.Web.Logging;
using RestSharp;

namespace NHS111.Models.Mappers.WebMappings
{
    public class AuditedModelMappers : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DosViewModel, AuditedDosRequest>();

            Mapper.CreateMap<ITKDispatchRequest, AuditedItkRequest>();

            Mapper.CreateMap<IRestResponse, AuditedItkResponse>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.StatusCode))
                .ForMember(dest => dest.IsSuccessStatusCode, opt => opt.MapFrom(src => src.IsSuccessful));

            Mapper.CreateMap<DosCheckCapacitySummaryResult, AuditedDosResponse>(MemberList.Destination)
                .ForMember(dest => dest.DosResultsContainItkOfferring, opt => opt.MapFrom(src => !src.ResultListEmpty && src.Success.Services.Any(s => s.OnlineDOSServiceType.IsReferral)))
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.Success))
                .ForMember(dest => dest.Error, opt => opt.MapFrom(src => src.Error));

            Mapper.CreateMap<SuccessObject<ServiceViewModel>, ResponseObject>(MemberList.Destination)
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code));

            Mapper.CreateMap<ErrorObject, ResponseObject>(MemberList.Destination)
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code));
        }
    }
}
