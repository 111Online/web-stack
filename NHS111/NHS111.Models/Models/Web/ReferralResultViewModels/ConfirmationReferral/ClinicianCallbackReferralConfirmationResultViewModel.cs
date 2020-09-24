namespace NHS111.Models.Models.Web
{
    public class ClinicianCallbackReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName { get { return string.Format("Confirmation/defaultWithDetails/Confirmation"); } }

        public ClinicianCallbackReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
        : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ClinicianCallbackReferralConfirmationAnalyticsDataLayer(this);
        }
    }
}
