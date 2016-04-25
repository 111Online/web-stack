using AutoMapper;
using NHS111.Models.Models.Web.Enums;

namespace NHS111.Models.Mappers.WebMappings
{
    internal class IntToAgeCategory : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<int, AgeCategory>()
                .ConvertUsing<FromIntToAgeCategoryConverter>();
        }

        public class FromIntToAgeCategoryConverter : ITypeConverter<int, AgeCategory>
        {
            public AgeCategory Convert(ResolutionContext context)
            {
                var age = (int) context.SourceValue;

                if (age >= 16) return AgeCategory.Adult;
                if (5 <= age && age <= 15) return AgeCategory.Child;
                if (1 <= age && age <= 4) return AgeCategory.Toddler;
                
                return AgeCategory.Infant;
            }
        }
    }
}
