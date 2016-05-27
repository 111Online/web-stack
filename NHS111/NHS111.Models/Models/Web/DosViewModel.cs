using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Web.Presentation.Models;

namespace NHS111.Models.Models.Web
{
    public class DosViewModel : DosCase
    {
        public Guid UserId { get; set; }
        public CheckCapacitySummaryResult[] CheckCapacitySummaryResultList { get; set; }
        public DosServicesByClinicalTermResult DosServicesByClinicalTermResult { get; set; }
        public string CheckCapacitySummaryResultListJson { get; set; }
        public IEnumerable<CareAdvice> CareAdvices { get; set; }
        public IEnumerable<string> CareAdviceMarkers { get; set; }
        public List<int> SearchDistances { get; set; }
        public string Title { get; set; }
        public string SelectedServiceId { get; set; }
        public string JourneyJson { get; set; }
        public string PathwayNo { get; set; }
        public CheckCapacitySummaryResult SelectedService
        {
            get { return CheckCapacitySummaryResultList.FirstOrDefault(s => s.IdField == Convert.ToInt32(SelectedServiceId)); }
        }

        public DosViewModel()
        {
            CareAdvices = new List<CareAdvice>();
            CareAdviceMarkers = new List<string>();
            SearchDistances = new List<int>() { 0, 10, 25, 50, 100 };
        }
    }
}