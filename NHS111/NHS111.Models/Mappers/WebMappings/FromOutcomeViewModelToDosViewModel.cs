using System;
using AutoMapper;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;

namespace NHS111.Models.Mappers.WebMappings
{
    using System.Linq;

    public class FromOutcomeViewModelToDosViewModel : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OutcomeViewModel, DosViewModel>()
                .ForMember(dest => dest.CareAdvices, opt => opt.MapFrom(src => src.CareAdvices))
                .ForMember(dest => dest.CheckCapacitySummaryResultList, opt => opt.MapFrom(src => src.CheckCapacitySummaryResultList))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CareAdviceMarkers, opt => opt.MapFrom(src => src.CareAdviceMarkers))
                .ForMember(dest => dest.CareAdvices, opt => opt.MapFrom(src => src.CareAdvices))
                .ForMember(dest => dest.CaseId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.JourneyJson, opt => opt.MapFrom(src => src.JourneyJson))
                .ForMember(dest => dest.PathwayNo, opt => opt.MapFrom(src => src.PathwayNo))
                .ForMember(dest => dest.SelectedServiceId, opt => opt.MapFrom(src => src.SelectedServiceId))
                .ForMember(dest => dest.SymptomDiscriminator, opt => opt.MapFrom(src => src.SymptomDiscriminator))
                .ForMember(dest => dest.SymptomGroup, opt => opt.MapFrom(src => src.SymptomGroup))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.PostCode,
                    opt => opt.ResolveUsing<PostcodeResolver>().FromMember(src => src.UserInfo))
                .ForMember(dest => dest.Disposition,
                    opt => opt.ResolveUsing<DispositionResolver>().FromMember(src => src.Id))
                .ForMember(dest => dest.SymptomDiscriminatorList,
                    opt => opt.ResolveUsing<SymptomDiscriminatorListResolver>().FromMember(dest => dest.SymptomDiscriminator))
                .ForMember(dest => dest.Gender,
                    opt => opt.ResolveUsing<GenderResolver>().FromMember(src => src.UserInfo.Gender))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.UserInfo.Age))
                .ForMember(dest => dest.Surgery, opt => opt.MapFrom(src => src.SurgeryViewModel.SelectedSurgery)); ;
        }

        public class DispositionResolver : ValueResolver<string, int>
        {
            protected override int ResolveCore(string source)
            {
                if (!source.StartsWith("Dx")) throw new FormatException("Dx code does not have prefix \"Dx\". Cannot convert");

                return Convert.ToInt32(source.Replace("Dx", "10"));
            }
        }


        public class PostcodeResolver : ValueResolver<UserInfo, string>
        {

            protected override string ResolveCore(UserInfo source)
            {
                return !string.IsNullOrEmpty(source.CurrentAddress.PostCode)
                   ? source.CurrentAddress.PostCode
                   : source.HomeAddress.PostCode;
            }
        }

        public class SymptomDiscriminatorListResolver : ValueResolver<string, int[]>
        {
            protected override int[] ResolveCore(string source)
            {
                if (source == null) return new int[0];
                int intVal = 0;
                if (!int.TryParse(source, out intVal)) throw new FormatException("Cannnot convert SymptomDiscriminator.  Not of integer format");

                return new[] { intVal };
            }
        }

        public class GenderResolver : ValueResolver<string, string>
        {
            protected override string ResolveCore(string source) {
                switch (source.ToLower()) {
                    case "female":
                        return "F";
                    case "male":
                        return "M";
                    default:
                        return "I";
                }
            }
        }
    }
}