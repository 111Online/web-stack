using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper.TypeConversion;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
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

            Mapper.CreateMap<List<JourneyStep>, List<ReportItem>>()
              .ConvertUsing<FromJourneySetpsToReportTextStrings>();

            Mapper.CreateMap<OutcomeViewModel, ITKConfirmationViewModel>()
                .ForMember(m => m.PatientReference, opt => opt.Ignore())
                .ForMember(m => m.ItkDuplicate, opt => opt.Ignore())
                .ForMember(m => m.ItkSendSuccess, opt => opt.Ignore());

        }
    }

    public class FromOutcomeViewModelToCaseDetailsConverter : ITypeConverter<OutcomeViewModel, CaseDetails>
    {
        public CaseDetails Convert(ResolutionContext context)
        {
            var outcome = (OutcomeViewModel)context.SourceValue;
            var caseDetails = (CaseDetails)context.DestinationValue ?? new CaseDetails();

            caseDetails.JourneyId = outcome.JourneyId.ToString();
            if (!outcome.HasAcceptedCallbackOffer.HasValue || !outcome.HasAcceptedCallbackOffer.Value) //callback was never offered or it was rejected
                caseDetails.DispositionCode = outcome.Id;
            else
                caseDetails.DispositionCode = FromOutcomeViewModelToDosViewModel.DispositionResolver.Remap(outcome.Id);
            caseDetails.DispositionName = outcome.Title;
            caseDetails.StartingPathwayTitle = outcome.PathwayTitle;
            caseDetails.StartingPathwayId = outcome.PathwayId;
            caseDetails.StartingPathwayType = outcome.PathwayTraumaType;
            caseDetails.ReportItems = Mapper.Map<List<JourneyStep>, List<ReportItem>>(outcome.Journey.Steps);
            caseDetails.ConsultationSummaryItems = outcome.Journey.Steps.Where(s => !string.IsNullOrEmpty(s.Answer.DispositionDisplayText)).Select(s => s.Answer.ReportText).Distinct().ToList();
            caseDetails.CaseSteps = outcome.Journey.Steps.Select(s => new StepItem() {QuestionId = s.QuestionId, AnswerOrder = s.Answer.Order});
            var state = outcome.Journey.GetLastState();
            caseDetails.SetVariables = !string.IsNullOrEmpty(state) ? JsonConvert.DeserializeObject<IDictionary<string, string>>(outcome.Journey.GetLastState()) : new Dictionary<string, string>();
            return caseDetails;
        }
    }

  

    public class FromJourneySetpsToReportTextStrings : ITypeConverter<List<JourneyStep>, List<ReportItem>>
    {
        public List<ReportItem> Convert(ResolutionContext context)
        {
            var steps = (List<JourneyStep>)context.SourceValue;
            return steps.Where(s => !string.IsNullOrEmpty(s.Answer.ReportText)).Select(s => new ReportItem { Text = s.Answer.ReportText, Positive = s.Answer.IsPositive }).ToList();
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
            patientDetails.CurrentAddress = MapAddress(personalDetailViewModel.AddressInformation.PatientCurrentAddress);
            if (personalDetailViewModel.AddressInformation.HomeAddressSameAsCurrent.HasValue)
            {
                if (personalDetailViewModel.AddressInformation.HomeAddressSameAsCurrent.Value ==
                    HomeAddressSameAsCurrent.Yes)
                {
                    patientDetails.HomeAddress =
                        MapAddress(personalDetailViewModel.AddressInformation.PatientCurrentAddress);
                }
                else if (personalDetailViewModel.AddressInformation.HomeAddressSameAsCurrent.Value ==
                         HomeAddressSameAsCurrent.No)
                {
                    patientDetails.HomeAddress =
                        MapAddress(personalDetailViewModel.AddressInformation.PatientHomeAddreess);
                }
            }
            if (personalDetailViewModel.UserInfo.Year != null && personalDetailViewModel.UserInfo.Month != null && personalDetailViewModel.UserInfo.Day != null)
                patientDetails.DateOfBirth =
                    new DateTime(personalDetailViewModel.UserInfo.Year.Value, personalDetailViewModel.UserInfo.Month.Value, personalDetailViewModel.UserInfo.Day.Value);

            patientDetails.Gender = personalDetailViewModel.UserInfo.Demography.Gender;
            var ageGroup = new AgeCategory(personalDetailViewModel.UserInfo.Demography.Age);
            patientDetails.AgeGroup = ageGroup.Value;

            patientDetails.Informant = new InformantDetails()
            {
                Forename = personalDetailViewModel.Informant.Forename,
                Surname = personalDetailViewModel.Informant.Surname,
                TelephoneNumber = personalDetailViewModel.UserInfo.TelephoneNumber,
                Type = personalDetailViewModel.Informant.IsInformantForPatient ? NHS111.Models.Models.Web.ITK.InformantType.NotSpecified : NHS111.Models.Models.Web.ITK.InformantType.Self
            };           
            
            return patientDetails;
        }

        private Address MapAddress(PersonalDetailsAddressViewModel addressViewModel)
        {
            return new Address()
            {
                PostalCode = addressViewModel.Postcode,
                StreetAddressLine1 =
                    !string.IsNullOrEmpty(addressViewModel.HouseNumber)
                        ? string.Format("{0} {1}", addressViewModel.HouseNumber, addressViewModel.AddressLine1)
                        : addressViewModel.AddressLine1,
                StreetAddressLine2 = addressViewModel.AddressLine2,
                StreetAddressLine3 = addressViewModel.AddressLine3,
                StreetAddressLine4 = addressViewModel.City,
                StreetAddressLine5 = addressViewModel.County
            };
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
            serviceDetails.Ccg = outcome.Source;
            serviceDetails.Stp = outcome.Campaign;
            
            return serviceDetails;
        }
    }
}
