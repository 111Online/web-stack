namespace NHS111.Models.Models.Web
{
    public class AccidentAndEmergencyDuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/DuplicateReferral"; } }

        public AccidentAndEmergencyDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencyDuplicateReferralAnalyticsDataLayer(this);
        }
    }
}
