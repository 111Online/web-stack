using AutoMapper;
using System;
using DayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromSystemDayOfWeekToDosDayOfWeek : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<System.DayOfWeek, DayOfWeek>()
                .ConvertUsing<FromSystemDayOfWeekToDosDayOfWeekConverter>();
        }

        public class FromSystemDayOfWeekToDosDayOfWeekConverter : ITypeConverter<System.DayOfWeek, DayOfWeek>
        {
            public DayOfWeek Convert(ResolutionContext context)
            {
                var source = (System.DayOfWeek)context.SourceValue;

                var sourceValue = (int)source;

                var converted = (DayOfWeek)sourceValue;

                if (!source.ToString().Equals(converted.ToString()))
                    throw new ArgumentOutOfRangeException("DayOfWeek", "Unable to map day of week to system type");

                return converted;
            }
        }
    }
}
