using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Book;
using NHS111.Models.Models.Web.ITK;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromPersonalDetailViewModelToBookAppointmentRequest : Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<PersonalDetailViewModel, BookAppointmentRequest>()
                .ForMember(dest => dest.SlotId, opt => opt.MapFrom(src => src.SelectedSlotId))
                .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.SelectedSlot.ScheduleId))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.SelectedSlot.Start))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.SelectedSlot.End))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.UserInfo))
                .ForMember(dest => dest.PractitionerId, opt => opt.MapFrom(src => src.SelectedSlot.PractitionerId));

            Mapper.CreateMap<UserInfo, Patient>()
                .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.TelephoneNumber, opt => opt.MapFrom(src => src.TelephoneNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DoB.Value))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Demography.Gender))
                .ForMember(dest => dest.Address, opt => opt.Ignore());

            Mapper.CreateMap<AddressInfoViewModel, Address>()
                .ForMember(dest => dest.StreetAddressLine1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForMember(dest => dest.StreetAddressLine2, opt => opt.MapFrom(src => src.AddressLine2))
                .ForMember(dest => dest.StreetAddressLine3, opt => opt.MapFrom(src => src.AddressLine3))
                .ForMember(dest => dest.StreetAddressLine4, opt => opt.Ignore())
                .ForMember(dest => dest.StreetAddressLine5, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Postcode));
        }
    }
}
