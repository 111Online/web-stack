namespace NHS111.Models.Models.Web
{
    public class CoronaReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public CoronaReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel) : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new CoronaReferralFailureAnalyticsDataLayer(this);
        }
    }
}
