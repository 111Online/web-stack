namespace NHS111.Models.Models.Web
{
    public class PharmacyReferralFailureResultsViewModel : ReferralFailureResultViewModel
    {
        public override string ViewName { get { return "Confirmation/Pharmacy/ServiceBookingFailure"; } }
        public override string PageTitle { get { return "Sorry, there's a problem with the service"; } }

        public PharmacyReferralFailureResultsViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new PharmacyReferralFailureAnalyticsDataLayer(this);
        }
    }
}
