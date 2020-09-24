namespace NHS111.Models.Models.Web
{
    public class ReferralFailureResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Call NHS 111 - request for callback not completed"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/ServiceBookingFailure", ResolveConfirmationViewByOutcome(this.ItkConfirmationModel)); } }
        public override string PartialViewName { get { return "_ReferralFailure"; } }

        public ReferralFailureResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ReferralFailureResultAnalyticsDataLayer(this);
        }
    }
}
