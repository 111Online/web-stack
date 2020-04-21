using AutoMapper;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.DosRequests;
using System;
using System.Linq;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromDosCaseToDosServicesByClinicalTermRequest : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<DosCase, DosServicesByClinicalTermRequest>()
                .ForMember(dest => dest.CaseId, opt => opt.MapFrom(src => src.CaseId))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(src => src.PostCode))
                .ForMember(dest => dest.SearchDistance, opt => opt.MapFrom(src => src.SearchDistance))
                .ForMember(dest => dest.Age,
                    opt => opt.ResolveUsing<AgeResolver>().FromMember(src => src.Age))
                .ForMember(dest => dest.Disposition,
                    opt => opt.ResolveUsing<DispositionResolver>().FromMember(src => src.Disposition))
                .ForMember(dest => dest.SymptomGroupDiscriminatorCombos, opt => opt.MapFrom(src => string.Format("{0}={1}", src.SymptomGroup, src.SymptomDiscriminator)))
                .ForMember(dest => dest.NumberPerType, opt => opt.MapFrom(src => src.NumberPerType))
                .ForMember(dest => dest.GpPracticeId, opt => opt.Ignore());
        }

        public class AgeResolver : ValueResolver<string, int>
        {
            protected override int ResolveCore(string source)
            {
                int age;
                if (!int.TryParse(source, out age)) return 1; //default to adult

                if (age >= 65) return 8;
                if (age >= 16 && age < 65) return 1;
                if (age >= 5 && age <= 15) return 2;
                if (age >= 1 && age <= 4) return 3;

                return 4;
            }
        }

        public class GenderResolver : ValueResolver<GenderEnum, string>
        {
            protected override string ResolveCore(GenderEnum source)
            {
                return source.ToString().ToCharArray().First().ToString();
            }
        }

        public class DispositionResolver : ValueResolver<int, string>
        {
            protected override string ResolveCore(int source)
            {
                if (source.ToString().Length < 2) throw new FormatException("Number must be greater than 100");
                if (!source.ToString().StartsWith("10")) throw new FormatException("Number must begin with 10");
                return string.Format("Dx{0}", source.ToString().Substring(2));
            }
        }
    }
}
