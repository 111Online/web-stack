namespace NHS111.Models.Models.Web
{
    public class ServiceFirstServiceUnavailableReferralResultViewModel : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_AccidentAndEmergencyServiceUnavailableReferral"; } }
        public override string ViewName
        {
            get
            {
                return !OutcomeModel.IsEmergencyPrescriptionOutcome ? "ServiceBookingUnavailable" : "Confirmation/Service_first/Emergency_Prescription/ServiceUnavailable";
            }
        }

        public ServiceFirstServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel) : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }
}
