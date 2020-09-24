namespace NHS111.Models.Models.Web
{
    public class ServiceFirstReferralFailureResultViewModel : ReferralFailureResultViewModel
    {
        public override string ViewName
        {
            get
            {
                var viewFolder = OutcomeModel.IsEDWithCallbackOffered ? "defaultWithDetails" : "default";
                return !OutcomeModel.IsEmergencyPrescriptionOutcome ? string.Format("Confirmation/{0}/ServiceBookingFailure", viewFolder) : "Confirmation/Service_first/Emergency_Prescription/ServiceBookingFailure";
            }
        }

        public ServiceFirstReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstReferralFailureAnalyticsDataLayer(this);
        }
    }
}
