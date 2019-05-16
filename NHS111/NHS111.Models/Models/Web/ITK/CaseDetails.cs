using System.Collections.Generic;

namespace NHS111.Models.Models.Web.ITK
{
    public class CaseDetails
    {
        public string ExternalReference { get; set; }
        public string StartingPathwayTitle { get; set; }
        public string StartingPathwayId { get; set; }
        public string StartingPathwayType { get; set; }
        public string DispositionCode { get; set; }
        public string DispositionName { get; set; }
        public List<ReportItem> ReportItems { get; set; }
        public List<string> ConsultationSummaryItems { get; set; }
        public IEnumerable<StepItem> CaseSteps { get; set; }
        public IDictionary<string, string> SetVariables { get; set; }
        public string PatientRef { get; set; }
    }
}
