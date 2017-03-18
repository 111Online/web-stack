using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Models.Models.Web.Logging;
using System.Net.Http;

namespace NHS111.Models.Mappers.WebMappings
{
    public class AuditedModelMappers : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DosViewModel, AuditedDosRequest>();

            Mapper.CreateMap<ITKDispatchRequest, AuditedItkRequest>();

            Mapper.CreateMap<HttpResponseMessage, AuditedItkResponse>();

            Mapper.CreateMap<DosCheckCapacitySummaryResult, AuditedDosResponse>();
        }
    }
}
