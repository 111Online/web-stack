namespace NHS111.Models.Models.Web
{
    public class PersonalDetailViewModel : OutcomeViewModel
    {
        public LocationInfoViewModel AddressInformation { get; set; }
        public PatientInformantViewModel PatientInformantDetails { get; set; }
        public EmailAddressViewModel EmailAddress { get; set; }
        public PersonalDetailViewModel()
        {
            AddressInformation = new LocationInfoViewModel();
            PatientInformantDetails = new PatientInformantViewModel();
            EmailAddress = new EmailAddressViewModel();
        }
    }
}
