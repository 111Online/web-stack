namespace NHS111.Models.Models.Web
{
    public class AccidentAndEmergencyReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/ServiceBookingFailure"; } }

        public AccidentAndEmergencyReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencyReferralFailureAnalyticsDataLayer(this);
        }
    }
}
