namespace NHS111.Models.Models.Web
{
    public class Coronavirus111CallbackReferralConfirmationResultsViewModel
        : ReferralConfirmationResultViewModel
    {

        public Coronavirus111CallbackReferralConfirmationResultsViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new Coronavirus111CallbackReferralConfirmationAnalyticsDataLayer(this);
        }
    }
}
