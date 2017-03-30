using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper.TypeConversion;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.ITK;
using ServiceDetails = NHS111.Models.Models.Web.ITK.ServiceDetails;

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

            Mapper.CreateMap<List<JourneyStep>, List<String>>()
              .ConvertUsing<FromJourneySetpsToReportTextStrings>();

        }
    }

    public class FromOutcomeViewModelToCaseDetailsConverter : ITypeConverter<OutcomeViewModel, CaseDetails>
    {
        public CaseDetails Convert(ResolutionContext context)
        {
            var outcome = (OutcomeViewModel)context.SourceValue;
            var caseDetails = (CaseDetails)context.DestinationValue ?? new CaseDetails();

            caseDetails.ExternalReference = outcome.SessionId.ToString();
            caseDetails.DispositionCode = outcome.Id;
            caseDetails.DispositionName = outcome.Title;
            caseDetails.Source = outcome.PathwayTitle;
            caseDetails.ReportItems = Mapper.Map<List<JourneyStep>, List<String>>(outcome.Journey.Steps);
            caseDetails.ConsultationSummaryItems = outcome.Journey.Steps.Where(s => !String.IsNullOrEmpty(s.Answer.DispositionDisplayText)).Select(s => s.Answer.ReportText).Distinct().ToList();
            return caseDetails;
        }
    }

    public class FromJourneySetpsToReportTextStrings : ITypeConverter<List<JourneyStep>, List<String>>
    {
        public List<String> Convert(ResolutionContext context)
        {
            var steps = (List<JourneyStep>)context.SourceValue;
            return steps.Where(s => !String.IsNullOrEmpty(s.Answer.ReportText)).Select(s => s.Answer.ReportText).ToList();
        }
    }

    public class FromOutcomeViewModelToPatientDetailsConverter : ITypeConverter<OutcomeViewModel, PatientDetails>
    {
        public PatientDetails Convert(ResolutionContext context)
        {
            var outcome = (OutcomeViewModel)context.SourceValue;
            var patientDetails = (PatientDetails)context.DestinationValue ?? new PatientDetails();

            patientDetails.Forename = outcome.UserInfo.FirstName;
            patientDetails.Surname = outcome.UserInfo.LastName;
            patientDetails.ServiceAddressPostcode = outcome.SelectedService.PostCode;
            patientDetails.TelephoneNumber = outcome.UserInfo.TelephoneNumber;
            patientDetails.CurrentAddress = new Address()
            {
                PostalCode = string.IsNullOrEmpty(outcome.AddressInfoViewModel.Postcode) ? null : outcome.AddressInfoViewModel.Postcode,
                StreetAddressLine1 =
                    !string.IsNullOrEmpty(outcome.AddressInfoViewModel.HouseNumber)
                        ? string.Format("{0} {1}", outcome.AddressInfoViewModel.HouseNumber, outcome.AddressInfoViewModel.AddressLine1)
                        : outcome.AddressInfoViewModel.AddressLine1,
                StreetAddressLine2 = outcome.AddressInfoViewModel.AddressLine2,
                StreetAddressLine3 = outcome.AddressInfoViewModel.City,
                StreetAddressLine4 = outcome.AddressInfoViewModel.County,
                StreetAddressLine5 = outcome.AddressInfoViewModel.Postcode
            };
            if (outcome.UserInfo.Year != null && outcome.UserInfo.Month != null && outcome.UserInfo.Day != null)
                patientDetails.DateOfBirth =
                    new DateTime(outcome.UserInfo.Year.Value, outcome.UserInfo.Month.Value, outcome.UserInfo.Day.Value);

            patientDetails.Gender = outcome.UserInfo.Demography.Gender;

            InformantType informantType;
            patientDetails.Informant = new InformantDetails()
            {
                Forename = outcome.Informant.Forename,
                Surname = outcome.Informant.Surname,
                TelephoneNumber = outcome.Informant.TelephoneNumber,
                Type = Enum.TryParse(outcome.Informant.Type, false, out informantType) ? informantType : InformantType.Self
            };
            
            return patientDetails;
        }
    }

    public class FromOutcomeViewModelToServiceDetailsConverter : ITypeConverter<OutcomeViewModel, ServiceDetails>
    {
        public ServiceDetails Convert(ResolutionContext context)
        {
            var outcome = (OutcomeViewModel)context.SourceValue;
            var serviceDetails = (ServiceDetails)context.DestinationValue ?? new ServiceDetails();

            serviceDetails.Id = outcome.SelectedService.Id.ToString();
            serviceDetails.Name = outcome.SelectedService.Name;
            serviceDetails.OdsCode = outcome.SelectedService.OdsCode;
            serviceDetails.PostCode = outcome.SelectedService.PostCode;

            return serviceDetails;
        }
    }
}
