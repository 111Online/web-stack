using System;
using System.Diagnostics;
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
                .ForMember(dest => dest.DosCheckCapacitySummaryResult,
                    opt => opt.MapFrom(src => src.DosCheckCapacitySummaryResult))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CareAdviceMarkers, opt => opt.MapFrom(src => src.CareAdviceMarkers))
                .ForMember(dest => dest.CareAdvices, opt => opt.MapFrom(src => src.CareAdvices))
                .ForMember(dest => dest.CaseId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.JourneyJson, opt => opt.MapFrom(src => src.JourneyJson))
                .ForMember(dest => dest.PathwayNo, opt => opt.MapFrom(src => src.PathwayNo))
                .ForMember(dest => dest.SelectedServiceId, opt => opt.MapFrom(src => src.SelectedServiceId))
                .ForMember(dest => dest.SymptomDiscriminator, opt => opt.MapFrom(src => src.SymptomDiscriminatorCode))
                .ForMember(dest => dest.SymptomGroup, opt => opt.MapFrom(src => src.SymptomGroup))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.SessionId))
                .ForMember(dest => dest.PostCode,
                    opt => opt.ResolveUsing<PostcodeResolver>().FromMember(src => src.UserInfo))
                .ForMember(dest => dest.Disposition,
                    opt => opt.ResolveUsing<DispositionResolver>().FromMember(src => src.Id))
                .ForMember(dest => dest.SymptomDiscriminatorList,
                    opt =>
                        opt.ResolveUsing<SymptomDiscriminatorListResolver>()
                            .FromMember(dest => dest.SymptomDiscriminatorCode))
                .ForMember(dest => dest.Gender,
                    opt => opt.ResolveUsing<GenderResolver>().FromMember(src => src.UserInfo.Demography.Gender))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.UserInfo.Demography.Age))
                .ForMember(dest => dest.DispositionTime, opt => opt.MapFrom(src => src.DispositionTime))
                .ForMember(dest => dest.DispositionTimeFrameMinutes, opt => opt.MapFrom(src => src.TimeFrameMinutes));
        }

        public class DispositionResolver : ValueResolver<string, int>
        {
            protected override int ResolveCore(string source)
            {
                if (!source.StartsWith("Dx")) throw new FormatException("Dx code does not have prefix \"Dx\". Cannot convert");
                var code = source.Replace("Dx", "");
                if (code.Length == 3)
                    return Convert.ToInt32("11" + code);

                return Convert.ToInt32("10" + code);
            }
        }


        public class PostcodeResolver : ValueResolver<UserInfo, string>
        {

            protected override string ResolveCore(UserInfo source)
            {
                return !string.IsNullOrEmpty(source.CurrentAddress.Postcode)
                   ? source.CurrentAddress.Postcode
                   : source.HomeAddress.Postcode;
            }
        }

        public class SymptomDiscriminatorListResolver : ValueResolver<string, int[]>
        {
            protected override int[] ResolveCore(string source)
            {
                if (source == null) return new int[0];
                int intVal = 0;
                if (!int.TryParse(source, out intVal)) throw new FormatException("Cannnot convert SymptomDiscriminatorCode.  Not of integer format");

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