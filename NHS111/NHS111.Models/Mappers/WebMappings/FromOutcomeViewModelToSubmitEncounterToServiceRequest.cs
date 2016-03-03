using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.ITK;

namespace NHS111.Models.Mappers.WebMappings
{
    public class FromOutcomeViewModelToSubmitEncounterToServiceRequest : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OutcomeViewModel, CaseDetails>()
                .ConvertUsing<FromOutcomeViewModelToCaseDetailsConverter>();

            Mapper.CreateMap<OutcomeViewModel, PatientDetails>()
                .ConvertUsing<FromOutcomeViewModelToPatientDetailsConverter>();

            Mapper.CreateMap<OutcomeViewModel, ServiceDetails>()
                .ConvertUsing<FromOutcomeViewModelToServiceDetailsConverter>();

            Mapper.CreateMap<OutcomeViewModel, SubmitEncounterToServiceRequest>()
                .ForMember(dest => dest.CaseDetails, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.PatientDetails, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.ServiceDetails, opt => opt.MapFrom(src => src));
        }
    }

    public class FromOutcomeViewModelToCaseDetailsConverter : ITypeConverter<OutcomeViewModel, CaseDetails>
    {
        public CaseDetails Convert(ResolutionContext context)
        {
            var outcome = (OutcomeViewModel)context.SourceValue;
            var caseDetails = (CaseDetails)context.DestinationValue ?? new CaseDetails();

            caseDetails.DispositionCode = outcome.Id;
            caseDetails.DispositionName = outcome.Title;

            return caseDetails;
        }
    }

    public class FromOutcomeViewModelToPatientDetailsConverter : ITypeConverter<OutcomeViewModel, PatientDetails>
    {
        public PatientDetails Convert(ResolutionContext context)
        {
            var outcome = (OutcomeViewModel) context.SourceValue;
            var patientDetails = (PatientDetails) context.DestinationValue ?? new PatientDetails();

            patientDetails.Forename = outcome.UserInfo.FirstName;
            patientDetails.Surname = outcome.UserInfo.LastName;
            patientDetails.CurrentAddress = new Address()
            {
                StreetAddressLine1 =
                    !string.IsNullOrEmpty(outcome.UserInfo.CurrentAddress.HouseNumber)
                        ? $"{outcome.UserInfo.CurrentAddress.HouseNumber} {outcome.UserInfo.CurrentAddress.AddressLine1}"
                        : outcome.UserInfo.CurrentAddress.AddressLine1,
                StreetAddressLine2 = outcome.UserInfo.CurrentAddress.AddressLine2,
                StreetAddressLine3 = outcome.UserInfo.CurrentAddress.City,
                StreetAddressLine4 = outcome.UserInfo.CurrentAddress.County,
                StreetAddressLine5 = outcome.UserInfo.CurrentAddress.PostCode
            };
            patientDetails.HomeAddress = new Address()
            {
                StreetAddressLine1 =
                    !string.IsNullOrEmpty(outcome.UserInfo.HomeAddress.HouseNumber)
                        ? $"{outcome.UserInfo.HomeAddress.HouseNumber} {outcome.UserInfo.HomeAddress.AddressLine1}"
                        : outcome.UserInfo.HomeAddress.AddressLine1,
                StreetAddressLine2 = outcome.UserInfo.HomeAddress.AddressLine2,
                StreetAddressLine3 = outcome.UserInfo.HomeAddress.City,
                StreetAddressLine4 = outcome.UserInfo.HomeAddress.County,
                StreetAddressLine5 = outcome.UserInfo.HomeAddress.PostCode
            };
            if (outcome.UserInfo.Year != null && outcome.UserInfo.Month != null && outcome.UserInfo.Day != null)
                patientDetails.DateOfBirth = new DateOfBirth()
                {
                    DateOfBirthItem = new DateTime(outcome.UserInfo.Year.Value, outcome.UserInfo.Month.Value, outcome.UserInfo.Day.Value)
                };
            patientDetails.Gender = outcome.UserInfo.Gender;
            patientDetails.InformationType = "NotSpecified";

            return patientDetails;
        }
    }

    public class FromOutcomeViewModelToServiceDetailsConverter : ITypeConverter<OutcomeViewModel, ServiceDetails>
    {
        public ServiceDetails Convert(ResolutionContext context)
        {
            var outcome = (OutcomeViewModel)context.SourceValue;
            var serviceDetails = (ServiceDetails)context.DestinationValue ?? new ServiceDetails();

            serviceDetails.Id = outcome.SelectedService.IdField.ToString();
            serviceDetails.Name = outcome.SelectedService.NameField;
            serviceDetails.OdsCode = outcome.SelectedService.OdsCodeField;
            serviceDetails.Address = outcome.SelectedService.AddressField;
            serviceDetails.PostCode = outcome.SelectedService.PostcodeField;

            return serviceDetails;
        }
    }
}
