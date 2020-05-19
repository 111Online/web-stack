namespace NHS111.Models.Models.Web
{

    public class SendSmsOutcomeViewModel : OutcomeViewModel
    {
        public VerificationCodeInputViewModel VerificationCodeInput { get; set; }
        private string _mobileNumber;
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set { _mobileNumber = value.Replace(" ", ""); }
        }
        public int Age { get; set; }
        public int SymptomsStartedDaysAgo { get; set; }
        public bool LivesAlone { get; set; }
        public string AnswerInputValue { get; set; }
        public string SelectedAnswer { get; set; }
    }
}
