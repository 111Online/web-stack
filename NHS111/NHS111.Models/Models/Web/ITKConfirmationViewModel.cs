namespace NHS111.Models.Models.Web
{
    public class ITKConfirmationViewModel : PersonalDetailViewModel
    {
        public string PatientReference { get; set; }
        public bool? ItkSendSuccess { get; set; }
        public bool? ItkDuplicate { get; set; }
    }
}
