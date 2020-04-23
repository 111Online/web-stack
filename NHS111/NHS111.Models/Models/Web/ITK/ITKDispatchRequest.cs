namespace NHS111.Models.Models.Web.ITK
{
    public class ITKDispatchRequest
    {
        public Authentication Authentication { get; set; }
        public PatientDetails PatientDetails { get; set; }
        public ServiceDetails ServiceDetails { get; set; }
        public CaseDetails CaseDetails { get; set; }
    }
}
