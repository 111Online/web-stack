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
        public AddressSearchViewModel AddressSearchViewModel { get; set; }
        public bool? ItkSendSuccess { get; set; }
        public CareAdvice WorseningCareAdvice { get; set; }

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

        public OutcomeViewModel()
        {
            SurgeryViewModel = new SurgeryViewModel();
            CareAdvices = new List<CareAdvice>();
            CareAdviceMarkers = new List<string>();
            AddressSearchViewModel = new AddressSearchViewModel();
            DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult();
        }
    }
}