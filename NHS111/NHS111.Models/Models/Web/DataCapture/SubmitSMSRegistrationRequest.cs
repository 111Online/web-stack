namespace NHS111.Models.Models.Web.DataCapture
{
    public class SubmitSMSRegistrationRequest
    {
        public string JourneyId { get; set; }
        public string PostCode { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string SymptomsStarted { get; set; }
        public bool LiveAlone { get; set; }
        public string VerificationCodeInput { get; set; }
    }
}
