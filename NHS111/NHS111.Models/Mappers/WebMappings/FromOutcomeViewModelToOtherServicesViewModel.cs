using System;
using AutoMapper;
using NHS111.Models.Models.Web;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeViewModelToOtherServicesViewModel : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OutcomeViewModel, OtherServicesViewModel>().ForMember(o => o.OtherServices, opt => opt.Ignore());
        }
    }
}
