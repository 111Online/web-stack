using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromQuestionViewModelToAnswerState : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<QuestionViewModel, SelectedAnswerState>()
                .ForMember(dest => dest.SelectedAnswer, 
                    opt => opt.MapFrom(src => JsonConvert.DeserializeObject<Answer>(src.SelectedAnswer)))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.StateJson));
        }
    }
}
