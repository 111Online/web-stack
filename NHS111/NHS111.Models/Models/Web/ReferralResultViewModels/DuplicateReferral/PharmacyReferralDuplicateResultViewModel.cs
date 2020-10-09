namespace NHS111.Models.Models.Web
{
    public class PharmacyReferralDuplicateResultViewModel: PharmacyReferralConfirmationResultsViewModel
    {
        public override string ViewName { get { return "Confirmation/Pharmacy/Confirmation"; } }
        public override string PartialViewName { get { return "DuplicateReferral"; } }

        public PharmacyReferralDuplicateResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel) : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new PharmacyReferralDuplicateAnalyticsDataLayer(this);
        }
    }
}
