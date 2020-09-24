namespace NHS111.Models.Models.Web
{
    public class ReferralConfirmationResultViewModel
        : ReferralResultViewModel
    {
        public override string PageTitle { get { return "Referral Confirmed"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/Confirmation", ResolveConfirmationViewByOutcome(this.ItkConfirmationModel)); } }
        public override string PartialViewName { get { return "_ReferralConfirmation"; } }

        public ReferralConfirmationResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new ReferralConfirmationResultAnalyticsDataLayer(this);
        }
    }
}
