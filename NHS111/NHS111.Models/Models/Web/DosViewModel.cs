using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using StructureMap.Query;

namespace NHS111.Models.Models.Web
{
    public class DosViewModel
    {
        public Guid UserId { get; set; }
        public CheckCapacitySummaryResult[] CheckCapacitySummaryResultList { get; set; }
        public string CheckCapacitySummaryResultListJson { get; set; }
        public IEnumerable<CareAdvice> CareAdvices { get; set; }
        public IEnumerable<string> CareAdviceMarkers { get; set; }
        public string Title { get; set; }
        public string PostCode { get; set; }
        public int SearchDistance { get; set; }
        public List<int> SearchDistances { get; set; }
        public int Age { get; set; }
        public GenderEnum Gender { get; set; }
        public string Id { get; set; }
        public string SymptomGroup { get; set; }
        public string SymptomDiscriminator { get; set; }
        public string SelectedServiceId { get; set; }
        public string JourneyJson { get; set; }
        public string PathwayNo { get; set; }
        public string SelectedSurgery { get; set; }
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