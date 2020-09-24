namespace NHS111.Models.Models.Web
{
    public class ServiceFirstReferralConfirmationResultsViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName 
        { 
            get
            {
                var viewFolder = OutcomeModel.IsEDWithCallbackOffered ? "defaultWithDetails" : "default";
                return !OutcomeModel.IsEmergencyPrescriptionOutcome ? string.Format("Confirmation/{0}/Confirmation", viewFolder) : "Confirmation/Service_first/Emergency_Prescription/Confirmation";
            }
        }

        public ServiceFirstReferralConfirmationResultsViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstReferralConfirmationAnalyticsDataLayer(this);
        }
    }
}
