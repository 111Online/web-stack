namespace NHS111.Models.Models.Web
{
    public class Call999DuplicateReferralResultViewModel
        : DuplicateReferralResultViewModel
    {
        public override string ViewName { get { return "Confirmation/defaultWithDetails/DuplicateReferral"; } }

        public Call999DuplicateReferralResultViewModel(ITKConfirmationViewModel itkConfirmationViewModel)
            : base(itkConfirmationViewModel)
        {
            AnalyticsDataLayer = new Call999DuplicateReferralAnalyticsDataLayer(this);
        }
    }
}
