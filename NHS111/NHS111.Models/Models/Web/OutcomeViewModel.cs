using System;
using System.Collections.Generic;
using System.Linq;
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
        public AddressInfoViewModel AddressInfoViewModel { get; set; }
        public bool? ItkSendSuccess { get; set; }
        public CareAdvice WorseningCareAdvice { get; set; }
        public SymptomDiscriminator SymptomDiscriminator { get; set; }

        public string CurrentView { get; set; }

        public bool HasEndpointReasoning
        {
            get
            {
                return SymptomDiscriminator != null &&
                       !String.IsNullOrWhiteSpace(SymptomDiscriminator.ReasoningText);
            }
        }

        public DosService SelectedService
        {
            get
            {
                return DosCheckCapacitySummaryResult.Success != null ? DosCheckCapacitySummaryResult.Success.Services.FirstOrDefault(s => s.Id == Convert.ToInt32(SelectedServiceId)) : null;
            }
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

                var timeFrameText = !string.IsNullOrEmpty(TimeFrameText) ? string.Format(" within {0}", TimeFrameText) : string.Empty;

                var dispositionText = !string.IsNullOrEmpty(OutcomeGroup.Text) ? string.Format("{0} {1}{2}", !string.IsNullOrEmpty(reasoningText) ? "<br />You should" : "Your answers suggest you should", OutcomeGroup.Text, timeFrameText) : string.Empty;

                return string.Format("{0}{1}", reasoningText, dispositionText);
            }
        }

        public OutcomeViewModel()
        {
            SurgeryViewModel = new SurgeryViewModel();
            CareAdvices = new List<CareAdvice>();
            CareAdviceMarkers = new List<string>();
            AddressInfoViewModel = new AddressInfoViewModel();
            DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
        }
    }
}