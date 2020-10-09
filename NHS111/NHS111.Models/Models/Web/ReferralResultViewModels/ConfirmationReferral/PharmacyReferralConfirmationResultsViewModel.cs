namespace NHS111.Models.Models.Web
{
    public class PharmacyReferralConfirmationResultsViewModel
        : ReferralConfirmationResultViewModel
    {
        public override string ViewName { get { return string.Format("Confirmation/Pharmacy/Confirmation"); } }

        public PharmacyReferralConfirmationResultsViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new PharmacyReferralConfirmationAnalysticsDataLayer(this); 
        }
    }
}
