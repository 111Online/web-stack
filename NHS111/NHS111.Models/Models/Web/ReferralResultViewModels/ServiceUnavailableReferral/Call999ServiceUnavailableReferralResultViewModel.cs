namespace NHS111.Models.Models.Web
{
    public class Call999ServiceUnavailableReferralResultViewModel
        : ServiceUnavailableReferralResultViewModel
    {
        public override string PartialViewName { get { return "_Call999ServiceUnavailableReferral"; } }

        public Call999ServiceUnavailableReferralResultViewModel(PersonalDetailViewModel outcomeViewModel)
            : base(outcomeViewModel)
        {
            AnalyticsDataLayer = new Call999ServiceUnavailableReferralAnalyticsDataLayer(this);
        }
    }
}
