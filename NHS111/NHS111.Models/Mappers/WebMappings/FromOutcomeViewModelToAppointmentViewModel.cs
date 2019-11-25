using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeViewModelToAppointmentViewModel : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OutcomeViewModel, AppointmentViewModel>().ForMember(p => p.Slots, opt => opt.Ignore());
            ;
        }
    }
}
