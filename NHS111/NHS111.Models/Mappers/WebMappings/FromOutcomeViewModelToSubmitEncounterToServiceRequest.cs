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

            Mapper.CreateMap<PersonalDetailViewModel, PatientDetails>()
                .ConvertUsing<FromPersonalDetailViewModelToPatientDetailsConverter>();

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

            caseDetails.ExternalReference = outcome.JourneyId.ToString();
            caseDetails.DispositionCode = outcome.Id;
            caseDetails.DispositionName = outcome.Title;
            caseDetails.Source = outcome.PathwayTitle;
            caseDetails.ReportItems = Mapper.Map<List<JourneyStep>, List<string>>(outcome.Journey.Steps);
            caseDetails.ConsultationSummaryItems = outcome.Journey.Steps.Where(s => !string.IsNullOrEmpty(s.Answer.DispositionDisplayText)).Select(s => s.Answer.ReportText).Distinct().ToList();
            return caseDetails;
        }
    }

    public class FromJourneySetpsToReportTextStrings : ITypeConverter<List<JourneyStep>, List<string>>
    {
        public List<string> Convert(ResolutionContext context)
        {
            var steps = (List<JourneyStep>)context.SourceValue;
            return steps.Where(s => !string.IsNullOrEmpty(s.Answer.ReportText)).Select(s => s.Answer.ReportText).ToList();
        }
    }

    public class FromPersonalDetailViewModelToPatientDetailsConverter : ITypeConverter<PersonalDetailViewModel, PatientDetails>
    {
        public PatientDetails Convert(ResolutionContext context)
        {
            var personalDetailViewModel = (PersonalDetailViewModel)context.SourceValue;
            var patientDetails = (PatientDetails)context.DestinationValue ?? new PatientDetails();

            patientDetails.Forename = personalDetailViewModel.UserInfo.FirstName;
            patientDetails.Surname = personalDetailViewModel.UserInfo.LastName;
            patientDetails.ServiceAddressPostcode = personalDetailViewModel.SelectedService.PostCode;
            patientDetails.TelephoneNumber = personalDetailViewModel.UserInfo.TelephoneNumber;
            patientDetails.CurrentAddress = new Address()
            {
                PostalCode = string.IsNullOrEmpty(personalDetailViewModel.CurrentPostcode) ? null : personalDetailViewModel.CurrentPostcode,
                StreetAddressLine1 =
                    !string.IsNullOrEmpty(personalDetailViewModel.AddressInformation.PatientCurrentAddress.HouseNumber)
                        ? string.Format("{0} {1}", personalDetailViewModel.AddressInformation.PatientCurrentAddress.HouseNumber, personalDetailViewModel.AddressInformation.PatientCurrentAddress.AddressLine1)
                        : personalDetailViewModel.AddressInformation.PatientCurrentAddress.AddressLine1,
                StreetAddressLine2 = personalDetailViewModel.AddressInformation.PatientCurrentAddress.AddressLine2,
                StreetAddressLine3 = personalDetailViewModel.AddressInformation.PatientCurrentAddress.City,
                StreetAddressLine4 = personalDetailViewModel.AddressInformation.PatientCurrentAddress.County,
                StreetAddressLine5 = personalDetailViewModel.CurrentPostcode
            };
            if (personalDetailViewModel.UserInfo.Year != null && personalDetailViewModel.UserInfo.Month != null && personalDetailViewModel.UserInfo.Day != null)
                patientDetails.DateOfBirth =
                    new DateTime(personalDetailViewModel.UserInfo.Year.Value, personalDetailViewModel.UserInfo.Month.Value, personalDetailViewModel.UserInfo.Day.Value);

            patientDetails.Gender = personalDetailViewModel.UserInfo.Demography.Gender;
            
            patientDetails.Informant = new InformantDetails()
            {
                Forename = personalDetailViewModel.Informant.Forename,
                Surname = personalDetailViewModel.Informant.Surname,
                TelephoneNumber = personalDetailViewModel.UserInfo.TelephoneNumber,
                Type = personalDetailViewModel.Informant.IsInformantForPatient ? NHS111.Models.Models.Web.ITK.InformantType.NotSpecified : NHS111.Models.Models.Web.ITK.InformantType.Self
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
