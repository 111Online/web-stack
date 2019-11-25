using AutoMapper;
using NHS111.Models.Models.Web;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeViewModelToPersonalDetailViewModel : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OutcomeViewModel, PersonalDetailViewModel>()
                .ForMember(p => p.PatientInformantDetails, opt => opt.Ignore())
                .ForMember(p => p.Slots, opt => opt.Ignore())
                .ForMember(p => p.SelectedSlotId, opt => opt.Ignore());
        }
    }
}