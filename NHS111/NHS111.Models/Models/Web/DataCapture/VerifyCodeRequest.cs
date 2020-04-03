namespace NHS111.Models.Models.Web.DataCapture
{
    public class VerifyCodeRequest
    {
        public string MobilePhoneNumber { get; set; }
        public string VerificationCodeInput { get; set; }
    }
}
