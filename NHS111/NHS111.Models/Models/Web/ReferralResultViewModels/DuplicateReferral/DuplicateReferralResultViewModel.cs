namespace NHS111.Models.Models.Web
{
    public class DuplicateReferralResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Call NHS 111 - duplicate request for callback"; } }
        public override string ViewName { get { return "DuplicateBookingFailure"; } }
        public override string PartialViewName { get { return "_DuplicateReferral"; } }

        public DuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new DuplicateReferralResultAnalyticsDataLayer(this);
        }
    }
}
