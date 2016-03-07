namespace NHS111.Models.Models.Web.ITK
{
    public class SubmitEncounterToServiceRequest
    {
        public CaseDetails CaseDetails { get; set; }
        public PatientDetails PatientDetails { get; set; }
        public ServiceDetails ServiceDetails { get; set; }
        public bool SendToRepeatCaller { get; set; }
    }
}
