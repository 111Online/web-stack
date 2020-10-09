namespace NHS111.Models.Models.Web
{
    public class TemporaryReferralDuplicateReferralResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public TemporaryReferralDuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel) :
            base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new DuplicateReferralResultAnalyticsDataLayer(this);
        }
    }
}
