using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Web
{
    public class OutcomeViewModel : JourneyViewModel
    {
        public string SelectedServiceId { get; set; }
        public DosCheckCapacitySummaryResult DosCheckCapacitySummaryResult { get; set; }
        public SurgeryViewModel SurgeryViewModel { get; set; }
        public IEnumerable<CareAdvice> CareAdvices { get; set; }
        public IEnumerable<string> CareAdviceMarkers { get; set; }
        public Enums.Urgency Urgency { get; set; }
        public string SymptomGroup { get; set; }
      
        public CareAdvice WorseningCareAdvice { get; set; }
        public SymptomDiscriminator SymptomDiscriminator { get; set; }
        public DosService UnavailableSelectedService { get; set; }
        public List<GroupedDOSServices> GroupedDosServices { get; set; } 
        public string CurrentView { get; set; }

        public  SurveyLinkViewModel SurveyLink { get; set; }

        public InformantViewModel Informant { get; set; }

        public bool HasSearched
        {
            get { return DosCheckCapacitySummaryResult.Success != null || DosCheckCapacitySummaryResult.Error != null; }
        }
        
        public bool HasEndpointReasoning
        {
            get
            {
                return SymptomDiscriminator != null &&
                       !String.IsNullOrWhiteSpace(SymptomDiscriminator.ReasoningText);
            }
        }

        public ServiceViewModel SelectedService
        {
            get
            {
                return DosCheckCapacitySummaryResult.Success != null ? DosCheckCapacitySummaryResult.Success.Services.FirstOrDefault(s => s.Id == Convert.ToInt32(SelectedServiceId)) : null;
            }
        }

        public bool Is999Callback
        {
            get
            {
                return (this.OutcomeGroup.Is999NonUrgent
                       && (this.DosCheckCapacitySummaryResult.HasITKServices ||
                           string.IsNullOrEmpty(this.CurrentPostcode)));
            }
        }

        public bool IsEDCallback
        {
            get
            {
                return (this.OutcomeGroup.IsEDCallback
                        && (this.DosCheckCapacitySummaryResult.HasITKServices ||
                            string.IsNullOrEmpty(this.CurrentPostcode)));
            }
        }

        public ServiceViewModel RemoveFirstDOSService()
        {
            var service = this.DosCheckCapacitySummaryResult.Success.Services.FirstOrDefault();
            if (GroupedDosServices.Any())
            {
                if(GroupedDosServices[0].Services.Count > 1)
                    GroupedDosServices.First().Services.RemoveAt(0);
                else
                    GroupedDosServices.RemoveAt(0);
            }
            return service;
        }

        public bool DisplayWorseningCareAdvice
        {
            get
            {
                return WorseningCareAdvice != null &&
                       this.CollectedKeywords.ExcludeKeywords.All(k => k.Value != WorseningCareAdvice.Keyword);
            }
        }

        public string DispositionText
        {
            get
            {
                var reasoningText = string.Empty;
                if (HasEndpointReasoning)
                    reasoningText = string.Format("From your answers, {0}", SymptomDiscriminator.ReasoningText);

                var timeFrameText = string.Empty;

                if (!(String.IsNullOrEmpty(TimeFrameText)))
                {
                    var preposition = Regex.IsMatch(TimeFrameText, @"\s*^[0-9]") ? "within " : String.Empty;
                    timeFrameText = string.Format(" {0}{1}", preposition, TimeFrameText);
                }

                var dispositionText = !string.IsNullOrEmpty(OutcomeGroup.Text) ? string.Format("{0} {1}{2}", !string.IsNullOrEmpty(reasoningText) ? "<br />You should" : "Your answers suggest you should", OutcomeGroup.Text, timeFrameText) : string.Empty;

                return string.Format("{0}{1}", reasoningText, dispositionText);
            }
        }

        public bool? HasAcceptedCallbackOffer { get; set; }

        public RecommendedServiceViewModel RecommendedService { get; set; }


        public OutcomeViewModel()
        {
            SurgeryViewModel = new SurgeryViewModel();
            CareAdvices = new List<CareAdvice>();
            CareAdviceMarkers = new List<string>();
            DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
            SurveyLink = new SurveyLinkViewModel();
            Informant = new InformantViewModel();
            GroupedDosServices = new List<GroupedDOSServices>();
            WorseningCareAdvice = new CareAdvice(new List<CareAdviceText>());
        }
    }

}