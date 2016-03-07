namespace NHS111.Models.Models.Web.ITK
{
    public class CaseDetails
    {
        public string ExternalReference { get; set; }
        public string Source { get; set; }
        public string DispositionCode { get; set; }
        public string DispositionName { get; set; }
        public CaseSummary CaseSummary { get; set; }
        public string Provider { get; set; }
    }
}
