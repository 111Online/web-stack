namespace NHS111.Models.Models.Web
{
    public class ClinicianReferralFailureResultViewModel
        : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/ServiceBookingFailure"; } }

        public ClinicianReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ClinicianReferralFailureAnalyticsDataLayer(this);
        }
    }
}
