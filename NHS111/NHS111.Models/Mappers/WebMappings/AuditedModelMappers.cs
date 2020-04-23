using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using NHS111.Models.Models.Web.Logging;
using RestSharp;
using System;
using System.Linq;

namespace NHS111.Models.Mappers.WebMappings
{
    public class AuditedModelMappers : Profile
    {
        private static readonly char[] Digits = "0123456789".ToCharArray();

        protected override void Configure()
        {
            Mapper.CreateMap<ITKDispatchRequest, AuditedItkRequest>();

            Mapper.CreateMap<IRestResponse, AuditedItkResponse>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.StatusCode))
                .ForMember(dest => dest.IsSuccessStatusCode, opt => opt.MapFrom(src => src.IsSuccessful));

            Mapper.CreateMap<PublicAuditViewModel, AuditViewModel>(MemberList.Source);

            Mapper.CreateMap<DosViewModel, AuditedDosRequest>(MemberList.Destination)
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => GetPartialPostcode(src.PostCode)));

            Mapper.CreateMap<DosCheckCapacitySummaryResult, AuditedDosResponse>(MemberList.Destination)
                .ForMember(dest => dest.DosResultsContainItkOfferring, opt => opt.MapFrom(src => !src.ResultListEmpty && src.Success.Services.Any(s => s.OnlineDOSServiceType.IsReferral)))
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.Success))
                .ForMember(dest => dest.Error, opt => opt.MapFrom(src => src.Error));

            Mapper.CreateMap<SuccessObject<ServiceViewModel>, ResponseObject>(MemberList.Destination)
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code));

            Mapper.CreateMap<ErrorObject, ResponseObject>(MemberList.Destination)
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code));
        }

        public static string GetPartialPostcode(string postcode)
        {
            if (string.IsNullOrEmpty(postcode)) return postcode;

            var postcodeWithoutWhitespace = postcode.Replace(" ", "");
            var lastDigit = postcodeWithoutWhitespace.LastIndexOfAny(Digits);
            if (lastDigit == -1)
            {
                throw new ArgumentException("No digits!");
            }
            return (lastDigit < postcodeWithoutWhitespace.Length - 1) ? postcodeWithoutWhitespace.Substring(0, lastDigit) : postcodeWithoutWhitespace;
        }
    }
}
