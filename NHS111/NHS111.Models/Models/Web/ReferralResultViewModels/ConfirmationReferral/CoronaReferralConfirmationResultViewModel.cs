namespace NHS111.Models.Models.Web
{
    public class CoronaReferralConfirmationResultViewModel
        : ReferralConfirmationResultViewModel
    {
        public CoronaReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel) : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new CoronaReferralFailureAnalyticsDataLayer(this);
        }
    }
}
