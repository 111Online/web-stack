namespace NHS111.Models.Models.Web.DataCapture
{
    public class VerifySMSCodeRequest
    {
        public string MobilePhoneNumber { get; set; }
        public string VerificationCodeInput { get; set; }
    }
}
