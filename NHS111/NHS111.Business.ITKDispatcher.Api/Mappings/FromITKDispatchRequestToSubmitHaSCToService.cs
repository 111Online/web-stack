using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService;
using NHS111.Models.Models.Web.ITK;


namespace NHS111.Business.ITKDispatcher.Api.Mappings
{
    public class FromITKDispatchRequestToSubmitHaSCToService : Profile
    {
        public override string ProfileName
        {
            get { return "FromITKDispatchRequestToSubmitHaSCToService"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ITKDispatchRequest, SubmitHaSCToService>()
                .ForMember(dest => dest.SubmitEncounterToServiceRequest, opt => opt.MapFrom(src => src));
         
            Mapper.CreateMap<NHS111.Models.Models.Web.ITK.Authentication, NHS111.Business.ITKDispatcher.Api.ITKDispatcherSOAPService.Authentication>();

        }
    }
}