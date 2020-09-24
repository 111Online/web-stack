namespace NHS111.Models.Models.Web
{
    public class ServiceFirstDuplicateReferralResultViewModel : DuplicateReferralResultViewModel
    {
        public override string ViewName
        {
            get
            {
                var viewFolder = OutcomeModel.IsEDWithCallbackOffered ? "defaultWithDetails" : "default";
                return !OutcomeModel.IsEmergencyPrescriptionOutcome ? string.Format("Confirmation/{0}/DuplicateReferral", viewFolder) : "Confirmation/Service_first/Emergency_Prescription/DuplicateReferral";
            }
        }
        public override string PartialViewName { get { return "DuplicateReferral"; } }

        public ServiceFirstDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel) : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstDuplicateReferralAnalyticsDataLayer(this);
        }
    }
}
