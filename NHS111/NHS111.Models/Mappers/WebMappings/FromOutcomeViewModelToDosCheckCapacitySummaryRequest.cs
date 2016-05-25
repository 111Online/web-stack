using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Web.Presentation.Models;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeViewModelToDosCase : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OutcomeViewModel, DosCase>()
                .ForMember(dest => dest.PostCode,
                    opt => opt.ResolveUsing<PostcodeResolver>().FromMember(src => src.UserInfo))
                .ForMember(dest => dest.Disposition,
                    opt => opt.ResolveUsing<DispositionResolver>().FromMember(src => src.Id))
                .ForMember(dest => dest.SymptomDiscriminatorList,
                    opt => opt.ResolveUsing<SymptomDiscriminatorListResolver>().FromMember(dest => dest.SymptomDiscriminator))
                .ForMember(dest => dest.Gender, 
                    opt => opt.ResolveUsing<GenderResolver>().FromMember(src => src.UserInfo.Gender))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.UserInfo.Age));  

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
                int intVal =0;
                if (!int.TryParse(source, out intVal)) throw new FormatException("Cannnot convert SymptomDiscriminator.  Not of integer format");

                return new[] {intVal};
            }
        }

        public class GenderResolver : ValueResolver<string, GenderEnum>
        {
            protected override GenderEnum ResolveCore(string source)
            {
                var genderStr = source.FirstOrDefault();
                GenderEnum gender = GenderEnum.Undisclosed;
                if (!string.IsNullOrEmpty(genderStr.ToString()))
                {
                    if (!Enum.TryParse(genderStr.ToString(), out gender))
                        return GenderEnum.Undisclosed;
                }

                return gender;
            }
        }
    }
}
