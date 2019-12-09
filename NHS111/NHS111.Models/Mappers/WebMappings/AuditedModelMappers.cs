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

            Mapper.CreateMap<DosCheckCapacitySummaryResult, AuditedDosResponse>();

            Mapper.CreateMap<PublicAuditViewModel, AuditViewModel>();
        }
    }
}
