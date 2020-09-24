namespace NHS111.Models.Models.Web
{
    //Temporarily removed until status of Dupe bug is known https://trello.com/c/5hqJVLDv
    public class TemporaryServiceFirstDuplicateReferralResultViewModel
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

        public TemporaryServiceFirstDuplicateReferralResultViewModel(
            ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ServiceFirstDuplicateReferralAnalyticsDataLayer(this);
        }
    }
}
