namespace NHS111.Models.Models.Web
{
    public class Coronavirus111CallbackUnavailableReferralResultViewModel : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "ServiceUnavailable"; } }
        public override string ViewName { get { return string.Format("Confirmation/{0}/ServiceUnavailable", ResolveConfirmationViewByOutcome(this.OutcomeModel)); } }

        public Coronavirus111CallbackUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel) : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new Coronavirus111CallbackServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }
}
