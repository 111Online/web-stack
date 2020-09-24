namespace NHS111.Models.Models.Web
{
    public class AccidentAndEmergencyReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName { get { return string.Format("Confirmation/defaultWithDetails/Confirmation"); } }

        public AccidentAndEmergencyReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new AccidentAndEmergencyReferralConfirmationAnalyticsDataLayer(this);
        }
    }
}
