namespace NHS111.Models.Models.Web
{
    public class Call999ReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/ServiceBookingFailure"; } }

        public Call999ReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new Call999ReferralFailureAnalyticsDataLayer(this);
        }
    }
}
