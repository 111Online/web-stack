using System;
using System.Diagnostics;
using AutoMapper;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;

namespace NHS111.Models.Mappers.WebMappings
{
    using System.Configuration;
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
                .ForMember(dest => dest.CaseRef, opt => opt.MapFrom(src => src.JourneyId))
                .ForMember(dest => dest.CaseId, opt => opt.MapFrom(src => src.JourneyId))
                .ForMember(dest => dest.JourneyJson, opt => opt.MapFrom(src => src.JourneyJson))
                .ForMember(dest => dest.PathwayNo, opt => opt.MapFrom(src => src.PathwayNo))
                .ForMember(dest => dest.SelectedServiceId, opt => opt.MapFrom(src => src.SelectedServiceId))
                .ForMember(dest => dest.SymptomDiscriminator, opt => opt.MapFrom(src => src.SymptomDiscriminatorCode))
                .ForMember(dest => dest.SymptomGroup, opt => opt.MapFrom(src => src.SymptomGroup))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.SessionId))
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.CurrentPostcode))
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
                .ForMember(dest => dest.DispositionTimeFrameMinutes, opt => opt.MapFrom(src => src.TimeFrameMinutes))

                .ForMember(dest => dest.DosServicesByClinicalTermResult, opt => opt.Ignore())
                .ForMember(dest => dest.CheckCapacitySummaryResultListJson, opt => opt.Ignore())
                .ForMember(dest => dest.SearchDistances, opt => opt.Ignore())
                .ForMember(dest => dest.Dispositions, opt => opt.Ignore())
                .ForMember(dest => dest.AgeFormat, opt => opt.Ignore())
                .ForMember(dest => dest.SearchDistance, opt => opt.Ignore())
                .ForMember(dest => dest.NumberPerType, opt => opt.Ignore())
                .ForMember(dest => dest.SearchDateTime, opt => opt.Ignore() );
        }

        public class DispositionResolver : ValueResolver<string, int>
        {
            protected override int ResolveCore(string source) {
                source = Remap(source);

                return ConvertToDosCode(source);
            }

            public static int ConvertToDosCode(string source) {
                if (!source.StartsWith("Dx")) throw new FormatException("Dx code does not have prefix \"Dx\". Cannot convert");
                var code = source.Replace("Dx", "");
                code = code.Replace("CV", "");
                if (code.Length == 3) {
                    if (code.StartsWith("1"))
                        return Convert.ToInt32("1" + code);
                    else
                        return Convert.ToInt32("11" + code);
                }

                return Convert.ToInt32("10" + code);
            }

            public static string Remap(string source) {

                var dictionary = ConfigurationManager.AppSettings["ValidationDxRemap"].Split(',').ToDictionary(k => k.Split(':').First(), v => v.Split(':').Last());

                if (dictionary.ContainsKey(source))
                {
                    return dictionary[source];
                }

                return source;
            }

            public static bool IsDOSRetry(string dxCode)
            {
                var mappingsForDxDosRetry = ConfigurationManager.AppSettings["ValidationDxRetry"];
                if (mappingsForDxDosRetry != null)
                {
                    var remappedCodes = mappingsForDxDosRetry.Split(',');
                    if (remappedCodes.Contains(dxCode))
                        return true;
                }

                return false;
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