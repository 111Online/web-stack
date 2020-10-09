namespace NHS111.Models.Models.Web
{
    public class Coronavirus111CallbackReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/default/ServiceBookingFailure"; } }

        public Coronavirus111CallbackReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new Coronavirus111CallbackReferralFailureAnalyticsDataLayer(this);
        }
    }
}
