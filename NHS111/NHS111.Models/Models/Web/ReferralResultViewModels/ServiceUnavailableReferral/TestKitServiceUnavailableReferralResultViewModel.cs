namespace NHS111.Models.Models.Web
{
    public class TestKitServiceUnavailableReferralResultViewModel : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "ServiceUnavailable"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/ServiceUnavailable", ResolveConfirmationViewByOutcome(this.OutcomeModel)); } }

        public TestKitServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel) : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new TestKitServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }
}
