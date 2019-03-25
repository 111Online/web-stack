using AutoMapper;
using NHS111.Models.Models.Web;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromServiceViewModelToRecommendedServiceViewModelMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ServiceViewModel, RecommendedServiceViewModel>()
                .ForMember(src => src.ReasonText, opt => opt.Ignore())
                .ForMember(src => src.Details, opt => opt.Ignore());
        }
    }
}
