using System;

namespace NHS111.Models.Models.Web.Logging
{
    public class AuditedDosRequest
    {
        public string PostCode { get; set; }
        public string PathwayNo { get; set; }
        public DateTime DispositionTime { get; set; }
        public int DispositionTimeFrameMinutes { get; set; }
        public string CaseId { get; set; }
        public string Surgery { get; set; }
        public string Age { get; set; }
        public int AgeFormat { get; set; }
        public int Disposition { get; set; }
        public int SymptomGroup { get; set; }
        public int SymptomDiscriminator { get; set; }
        public bool SearchDistanceSpecified { get; set; }
        public string Gender { get; set; }
        public int NumberPerType { get; set; }
    }
}
