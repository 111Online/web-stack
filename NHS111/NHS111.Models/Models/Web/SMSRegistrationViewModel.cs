namespace NHS111.Models.Models.Web
{
    public class SMSRegistrationViewModel
    {
        public string ViewName { get; set; }

        public SendSmsOutcomeViewModel SendSmsOutcomeViewModel;

        public SMSRegistrationViewModel(SendSmsOutcomeViewModel sendSmsOutcomeViewModelModel)
        {
            SendSmsOutcomeViewModel = sendSmsOutcomeViewModelModel;
        }
    }
}
