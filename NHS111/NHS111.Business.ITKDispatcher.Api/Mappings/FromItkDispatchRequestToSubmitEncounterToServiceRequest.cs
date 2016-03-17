using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;
using NHS111.Models.Models.Web.ITK;

namespace NHS111.Business.ITKDispatcher.Api.Mappings
{
    public class FromItkDispatchRequestToSubmitEncounterToServiceRequest : Profile
    {
        public override string ProfileName
        {
            get { return "FromItkDispatchRequestToSubmitEncounterToServiceRequest"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap <Models.Models.Web.ITK.Address, ITKDispatcherSOAPService.Address>();
            Mapper.CreateMap<Models.Models.Web.ITK.GpPractice, ITKDispatcherSOAPService.GPPractice>();
            Mapper.CreateMap<Models.Models.Web.ITK.ServiceDetails, ITKDispatcherSOAPService.SubmitToServiceDetails>();

            Mapper.CreateMap<ITKDispatchRequest, SubmitEncounterToServiceRequest>();
            Mapper.CreateMap<CaseDetails, SubmitToCallQueueDetails>();
            Mapper.CreateMap<NHS111.Models.Models.Web.ITK.PatientDetails, SubmitPatientService>()
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => new DateOfBirth() {Item = src.DateOfBirth.ToString("yyyy-MM-dd")}))
                .ForMember(src => src.InformantType, opt => opt.UseValue(informantType.Self))
                .ForMember(src => src.CurrentAddress, opt => opt.MapFrom(src => new ITKDispatcherSOAPService.Address(){PostalCode  = src.CurrentLocationPostcode}));



        }
    }
}